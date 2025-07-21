using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events.CurrencyDepositedHandlers;

public class AddToOutbox(IOutboxRepository outboxRepository) : IDomainEventHandler<CurrencyDeposited>
{
    public Task Handle(CurrencyDeposited notification, CancellationToken cancellationToken)
    {
        return outboxRepository.Add(
            notification,
            new CurrencyDepositedIntegrationEvent
            {
                Amount = notification.Amount,
                CurrencyId = notification.CurrencyId,
                PortfolioId = notification.PortfolioId
            },
            notification.PortfolioId.ToString(),
            cancellationToken);
    }
}