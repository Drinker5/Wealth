using Microsoft.Extensions.Logging;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.BuildingBlocks.Infrastructure.CommandScheduler;

internal class CommandsScheduler : ICommandsScheduler
{
    private readonly ILogger<CommandsScheduler> logger;
    private readonly IDeferredOperationRepository repository;

    public CommandsScheduler(
        ILogger<CommandsScheduler> logger,
        IDeferredOperationRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public Task EnqueueAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var outboxMessage = DefferedCommand.Create(
            Clock.Now,
            command);

        return AddDefferedCommand(outboxMessage, cancellationToken);
    }

    public Task EnqueueAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var outboxMessage = DefferedCommand.Create(
            Clock.Now,
            command);

        return AddDefferedCommand(outboxMessage, cancellationToken);
    }

    public Task ScheduleAsync(ICommand command, DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        var outboxMessage = DefferedCommand.CreateDelayed(
            Clock.Now,
            command,
            date);

        return AddDefferedCommand(outboxMessage, cancellationToken);
    }

    private Task AddDefferedCommand(DefferedCommand outboxMessage, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("DefferedCommand added: {@Message}", outboxMessage);
        return repository.Add(outboxMessage, cancellationToken);
    }
}
