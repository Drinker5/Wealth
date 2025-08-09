using Wealth.Aggregation.Application.Commands;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Events;

public class StockBoughtIntegrationEventHandler(ICqrsInvoker cqrsInvoker) : IIntegrationEventHandler<StockBoughtIntegrationEvent>
{
    public Task Handle(StockBoughtIntegrationEvent @event, CancellationToken token = default)
    {
        return cqrsInvoker.Command(new BuyStock
        {
            StockId = @event.StockId,
            Quantity = @event.Quantity,
            TotalPrice = @event.TotalPrice,
        });
    }
}