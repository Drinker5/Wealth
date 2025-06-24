using System.Collections.Concurrent;
using System.Net.Sockets;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Abstractions;
using Wealth.CurrencyManagement.Domain.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

internal class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
{
    private readonly IOutboxRepository outboxRepository;
    private readonly IOptions<OutboxPollingOptions> options;
    private readonly IJsonSerializer jsonSerializer;
    private readonly ILogger<ProcessOutboxCommandHandler> logger;
    private readonly IServiceProvider serviceProvider;
    private int _executedTimes;
    private readonly ConcurrentDictionary<string, Type?> _typeCache;
    private readonly ResiliencePipeline _pipeline;

    public ProcessOutboxCommandHandler(
        IOutboxRepository outboxRepository,
        IOptions<OutboxPollingOptions> options,
        IJsonSerializer jsonSerializer,
        ILogger<ProcessOutboxCommandHandler> logger,
        IServiceProvider serviceProvider)
    {
        this.outboxRepository = outboxRepository;
        this.options = options;
        _pipeline = CreateResiliencePipeline(options.Value.RetryCount);
        this.jsonSerializer = jsonSerializer;
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        _executedTimes = 0;
        _typeCache = new();
    }

    public async Task Handle(ProcessOutboxCommand request, CancellationToken cancellationToken)
    {
        var outboxMessage = await outboxRepository.LoadAsync(request.MessageId, cancellationToken);

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

    private async Task ProcessCommandAndDeleteAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken)
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
            await mediator.Publish((deserializedMessage as IDomainEvent)!, cancellationToken);
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Commit(cancellationToken);
        }
        else if (type.IsAssignableTo(typeof(ICommand)) ||
                 type.IsAssignableTo(typeof(ICommand<>)))
        {
            await mediator.Send(deserializedMessage, cancellationToken);
        }
        else if (type.IsAssignableTo(typeof(IntegrationEvent)))
        {
            var publishIntegrationEventCommand = new PublishIntegrationEventCommand((IntegrationEvent)deserializedMessage);
            await mediator.Send(publishIntegrationEventCommand, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException($"Cound not handle type '{type}'");
        }

        outboxRepository.Remove(outboxMessage);
    }

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        // See https://www.pollydocs.org/strategies/retry.html
        var retryOptions = new RetryStrategyOptions
        {
            MaxRetryAttempts = retryCount,
            ShouldHandle = new PredicateBuilder()
                .Handle<ArgumentException>(_ => false)
                .Handle<Exception>(e => e is not ArgumentException),
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