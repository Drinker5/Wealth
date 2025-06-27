using MediatR;

namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface IQuery<out TResult> : IRequest<TResult>, IQuery
{
}

public interface IQuery;