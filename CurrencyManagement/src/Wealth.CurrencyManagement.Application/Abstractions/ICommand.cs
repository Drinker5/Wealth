using MediatR;

namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface ICommand<out TResult> : IRequest<TResult>, ICommand
{
}

public interface ICommand : IRequest
{
}