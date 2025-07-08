using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events;

public class CurrencyAddedEventHandler(IOutboxRepository outboxRepository) : IDomainEventHandler<CurrencyAdded>
{
    public Task Handle(CurrencyAdded notification, CancellationToken cancellationToken)
    {
        return outboxRepository.Add(notification, cancellationToken);
    }
}