namespace Wealth.BuildingBlocks.Application;

public interface ICqrsInvoker
{
    Task<TResult> Command<TResult>(ICommand<TResult> command);
    Task Command(ICommand command);
    Task<TResult> Query<TResult>(IQuery<TResult> query);
}