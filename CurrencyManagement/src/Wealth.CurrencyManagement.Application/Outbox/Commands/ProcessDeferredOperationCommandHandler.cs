using System.Collections.Concurrent;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Application.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

internal class ProcessDeferredOperationCommandHandler : ICommandHandler<ProcessDeferredOperationCommand>
{
    private readonly IDeferredOperationRepository deferredOperationRepository;
    private readonly IJsonSerializer jsonSerializer;
    private readonly ILogger<ProcessDeferredOperationCommandHandler> logger;
    private readonly IServiceProvider serviceProvider;
    private int _executedTimes;
    private readonly ConcurrentDictionary<string, Type?> _typeCache;
    private readonly ResiliencePipeline _pipeline;

    public ProcessDeferredOperationCommandHandler(
        IDeferredOperationRepository deferredOperationRepository,
        IOptions<DeferredOperationPollingOptions> options,
        IJsonSerializer jsonSerializer,
        ILogger<ProcessDeferredOperationCommandHandler> logger,
        IServiceProvider serviceProvider)
    {
        this.deferredOperationRepository = deferredOperationRepository;
        _pipeline = CreateResiliencePipeline(options.Value.RetryCount);
        this.jsonSerializer = jsonSerializer;
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        _executedTimes = 0;
        _typeCache = new();
    }

    public async Task Handle(ProcessDeferredOperationCommand request, CancellationToken cancellationToken)
    {
        var outboxMessage = await deferredOperationRepository.LoadAsync(request.OperationId, cancellationToken);

        if (outboxMessage is null)
        {
            logger.LogWarning("outbox message not found! skipping.");
            return;
        }

        var context = ResilienceContextPool.Shared.Get(cancellationToken);
        var outcome = await _pipeline.ExecuteOutcomeAsync(async (context, outboxMessage) =>
        {
            try
            {
                await ProcessCommandAndDeleteAsync(outboxMessage, context.CancellationToken);
                return Outcome.FromResult(true);
            }
            catch (Exception ex)
            {
                return Outcome.FromException<bool>(ex);
            }
        }, context, outboxMessage);

        if (outcome.Exception is not null)
        {
            logger.LogError("failed to process outbox message.");
            outboxMessage.Error = outcome.Exception.ToString();
            outboxMessage.ProcessedDate = Clock.Now;
        }
    }

    private async Task ProcessCommandAndDeleteAsync(DefferedCommand outboxMessage, CancellationToken cancellationToken)
    {
        _executedTimes++;
        if (_executedTimes > 1)
            logger.LogWarning("OutboxMessage of type: {Type} executed {Times} times", outboxMessage.Type, _executedTimes);

        Type? type = _typeCache.GetOrAdd(outboxMessage.Type, typeAsStr =>
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == outboxMessage.AssemblyName);
            return assembly?.GetType(typeAsStr);
        });
        if (type is null)
        {
            throw new InvalidOperationException($"Could not find type '{outboxMessage.Type}'");
        }

        var deserializedMessage = jsonSerializer.Deserialize(outboxMessage.Data, type);
        if (deserializedMessage is null)
            throw new InvalidOperationException($"Could not deserialize message '{outboxMessage.Data}'");

        await using var scope = serviceProvider.CreateAsyncScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        if (type.IsAssignableTo(typeof(INotification)))
        {
            await mediator.Publish((deserializedMessage as DomainEvent)!, cancellationToken);
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Commit(cancellationToken);
        }
        else if (type.IsAssignableTo(typeof(ICommand)) ||
                 type.IsAssignableTo(typeof(ICommand<>)))
        {
            await mediator.Send(deserializedMessage, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException($"Cound not handle type '{type}'");
        }

        deferredOperationRepository.Remove(outboxMessage);
    }

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        // See https://www.pollydocs.org/strategies/retry.html
        var retryOptions = new RetryStrategyOptions
        {
            MaxRetryAttempts = retryCount,
            DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();

        static TimeSpan? GenerateDelay(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }
}