namespace Wealth.BuildingBlocks.Application.CommandScheduler;

public interface ICommandsScheduler
{
    Task EnqueueAsync(ICommand command, CancellationToken cancellationToken = default);
    Task EnqueueAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task ScheduleAsync(ICommand command, DateTimeOffset date, CancellationToken cancellationToken = default);
}