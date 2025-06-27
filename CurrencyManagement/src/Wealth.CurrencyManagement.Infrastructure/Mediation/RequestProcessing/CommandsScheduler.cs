using Microsoft.Extensions.Logging;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Utilities;

namespace Wealth.CurrencyManagement.Infrastructure.Mediation.RequestProcessing;

internal class CommandsScheduler : ICommandsScheduler
{
    private readonly ILogger<CommandsScheduler> logger;
    private readonly IJsonSerializer jsonSerializer;
    private readonly IOutboxRepository repository;

    public CommandsScheduler(
        ILogger<CommandsScheduler> logger,
        IJsonSerializer jsonSerializer,
        IOutboxRepository repository)
    {
        this.logger = logger;
        this.jsonSerializer = jsonSerializer;
        this.repository = repository;
    }

    public async Task EnqueueAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var outboxMessage = OutboxMessage.Create(
            jsonSerializer,
            Clock.Now,
            command);

        await AddOutboxMessageAsync(outboxMessage, cancellationToken);
    }

    public async Task EnqueueAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var outboxMessage = OutboxMessage.Create(
            jsonSerializer,
            Clock.Now,
            command);

        await AddOutboxMessageAsync(outboxMessage, cancellationToken);
    }

    public async Task EnqueuePublishingEventAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var outboxMessage = OutboxMessage.Create(
            jsonSerializer,
            Clock.Now,
            integrationEvent);

        await AddOutboxMessageAsync(outboxMessage, cancellationToken);
    }

    public async Task ScheduleAsync(ICommand command, DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        var outboxMessage = OutboxMessage.CreateDelayed(
            jsonSerializer,
            Clock.Now,
            command,
            date);

        await AddOutboxMessageAsync(outboxMessage, cancellationToken);
    }

    private async Task AddOutboxMessageAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("OutboxMessage added: {@Message}", outboxMessage);
        await repository.Add(outboxMessage, cancellationToken);
    }
}
