using MediatR;

namespace Wealth.BuildingBlocks.Application;

public interface IQueryHandler<TQuery, TResult> :
    IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}