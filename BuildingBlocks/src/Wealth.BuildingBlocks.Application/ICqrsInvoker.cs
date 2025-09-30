namespace Wealth.BuildingBlocks.Application;

public interface ICqrsInvoker
{
    Task<TResult> Command<TResult>(ICommand<TResult> command, CancellationToken token = default);
    Task Command(ICommand command, CancellationToken token = default);
    Task<TResult> Query<TResult>(IQuery<TResult> query);
}