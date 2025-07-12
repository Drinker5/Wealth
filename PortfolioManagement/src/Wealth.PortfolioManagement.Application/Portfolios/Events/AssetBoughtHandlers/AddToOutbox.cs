using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events.AssetBoughtHandlers;

public class AddToOutbox(IOutboxRepository outboxRepository) : IDomainEventHandler<AssetBought>
{
    public Task Handle(AssetBought notification, CancellationToken cancellationToken)
    {
        return outboxRepository.Add(notification, cancellationToken);
    }
}