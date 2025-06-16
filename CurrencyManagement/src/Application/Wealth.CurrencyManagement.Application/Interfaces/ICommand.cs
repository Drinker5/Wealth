using MediatR;

namespace Wealth.CurrencyManagement.Application.Interfaces;

public interface ICommand<out TResult> : IRequest<TResult>
{
}

public interface ICommand : IRequest
{
}