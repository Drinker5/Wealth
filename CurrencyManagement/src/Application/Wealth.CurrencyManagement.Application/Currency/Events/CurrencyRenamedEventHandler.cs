using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Currency;

namespace Wealth.CurrencyManagement.Application.Currency.Events;

public class CurrencyRenamedEventHandler : IDomainEventHandler<CurrencyRenamed>
{
    public Task Handle(CurrencyRenamed notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
