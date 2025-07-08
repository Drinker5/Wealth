using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events;

public class AssetAddedEventHandler(IOutboxRepository outboxRepository) : IDomainEventHandler<AssetAdded>
{
    public Task Handle(AssetAdded notification, CancellationToken cancellationToken)
    {
        return outboxRepository.Add(notification, cancellationToken);
    }
}