using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Events;

public class StockBoughtIntegrationEventEventHandler:  IIntegrationEventHandler<StockBoughtIntegrationEvent>
{
    public Task Handle(StockBoughtIntegrationEvent @event, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}