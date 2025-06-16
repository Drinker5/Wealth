using MediatR;

namespace Wealth.CurrencyManagement.Application.Interfaces;

public interface IQueryHandler<TQuery, TResult> :
    IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}