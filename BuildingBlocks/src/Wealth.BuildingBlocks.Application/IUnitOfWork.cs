using MediatR;

namespace Wealth.BuildingBlocks.Application;

public interface IUnitOfWork
{
    Task<TResponse> Transaction<TResponse>(RequestHandlerDelegate<TResponse> next, CancellationToken token);
}