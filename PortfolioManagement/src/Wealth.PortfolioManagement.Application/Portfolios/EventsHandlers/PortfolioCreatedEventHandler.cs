using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.EventsHandlers;

public class PortfolioCreatedEventHandler : IDomainEventHandler<PortfolioCreated>
{
    public Task Handle(PortfolioCreated notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}