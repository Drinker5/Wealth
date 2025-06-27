using MediatR;

namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface IQueryHandler<TQuery, TResult> :
    IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}