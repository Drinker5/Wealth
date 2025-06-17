using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Currencies.Events;

public class CurrencyRenamedEventHandler : IDomainEventHandler<CurrencyRenamed>
{
    public Task Handle(CurrencyRenamed notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
