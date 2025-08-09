using Wealth.Aggregation.Application.Commands;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Events;

public class StockPriceChangedIntegrationEventHandler(ICqrsInvoker cqrsInvoker) : IIntegrationEventHandler<StockPriceChangedIntegrationEvent>
{
    public Task Handle(StockPriceChangedIntegrationEvent @event, CancellationToken token = default)
    {
        return cqrsInvoker.Command(new StockChangePrice
        {
            StockId = @event.StockId,
            NewPrice = @event.NewPrice,
        });
    }
}