using MediatR;

namespace Wealth.BuildingBlocks.Application;

public interface IQuery<out TResult> : IRequest<TResult>, IQuery
{
}

public interface IQuery;