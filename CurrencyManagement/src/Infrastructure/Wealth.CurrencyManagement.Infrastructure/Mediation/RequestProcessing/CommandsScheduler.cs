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

    public async Task EnqueueAsync(ICommand command)
    {
        var outboxMessage = OutboxMessage.Create(
            jsonSerializer,
            Clock.Now,
            command);

        await AddOutboxMessageAsync(outboxMessage);
    }

    public async Task EnqueueAsync<TResult>(ICommand<TResult> command)
    {
        var outboxMessage = OutboxMessage.Create(
            jsonSerializer,
            Clock.Now,
            command);

        await AddOutboxMessageAsync(outboxMessage);
    }

    public async Task EnqueuePublishingEventAsync(IntegrationEvent integrationEvent)
    {
        var outboxMessage = OutboxMessage.Create(
            jsonSerializer,
            Clock.Now,
            integrationEvent);

        await AddOutboxMessageAsync(outboxMessage);
    }

    public async Task ScheduleAsync(ICommand command, DateTimeOffset date)
    {
        var outboxMessage = OutboxMessage.CreateDelayed(
            jsonSerializer,
            Clock.Now,
            command,
            date);

        await AddOutboxMessageAsync(outboxMessage);
    }

    private async Task AddOutboxMessageAsync(OutboxMessage outboxMessage)
    {
        logger.LogInformation("OutboxMessage added: {@Message}", outboxMessage);
        await repository.Add(outboxMessage);
    }
}
