using Wealth.Aggregation.Application.Commands;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Events;

public sealed class StockPriceChangedIntegrationEventHandler(ICqrsInvoker cqrsInvoker)
    : IMessageHandler<StockPriceChangedIntegrationEvent>
{
    public Task Handle(StockPriceChangedIntegrationEvent @event, CancellationToken token = default)
    {
        return cqrsInvoker.Command(new StockChangePrice
        {
            StockId = @event.StockId,
            NewPrice = @event.NewPrice,
        }, token);
    }
}