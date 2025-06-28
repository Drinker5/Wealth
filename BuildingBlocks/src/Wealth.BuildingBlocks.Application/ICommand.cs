using MediatR;

namespace Wealth.BuildingBlocks.Application;

public interface ICommand<out TResult> : IRequest<TResult>, ICommand
{
}

public interface ICommand : IRequest
{
}