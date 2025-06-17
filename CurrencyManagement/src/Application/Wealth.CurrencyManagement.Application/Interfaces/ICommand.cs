using MediatR;

namespace Wealth.CurrencyManagement.Application.Interfaces;

public interface ICommand<out TResult> : IRequest<TResult>, ICommand
{
}

public interface ICommand : IRequest
{
}