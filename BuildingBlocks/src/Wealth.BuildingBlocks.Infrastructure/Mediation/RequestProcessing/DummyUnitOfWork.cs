using MediatR;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing;

internal class DummyUnitOfWork : IUnitOfWork
{
    public Task<TResponse> Transaction<TResponse>(RequestHandlerDelegate<TResponse> next, CancellationToken token) 
        => next(token);
}