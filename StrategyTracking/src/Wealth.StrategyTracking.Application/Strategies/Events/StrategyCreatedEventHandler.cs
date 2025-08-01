using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Strategies.Events;

namespace Wealth.StrategyTracking.Application.Strategies.Events;

public class StrategyCreatedEventHandler : IDomainEventHandler<StrategyCreated>
{
    public Task Handle(StrategyCreated notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}