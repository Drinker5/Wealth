using MediatR;

namespace Wealth.CurrencyManagement.Application.Interfaces;

public interface IQuery<out TResult> : IRequest<TResult>, IQuery
{
}

public interface IQuery;