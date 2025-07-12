using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events.CurrencyDepositedHandlers;

public class AddToOutbox(IOutboxRepository outboxRepository) : IDomainEventHandler<CurrencyDeposited>
{
    public Task Handle(CurrencyDeposited notification, CancellationToken cancellationToken)
    {
        return outboxRepository.Add(notification, cancellationToken);
    }
}