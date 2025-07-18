using Wealth.Aggregation.Application.Commands;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Events;

public class InstrumentPriceChangedIntegrationEventHandler(ICqrsInvoker cqrsInvoker) : IIntegrationEventHandler<InstrumentPriceChangedIntegrationEvent>
{
    public Task Handle(InstrumentPriceChangedIntegrationEvent @event, CancellationToken token = default)
    {
        if (@event.InstrumentType == InstrumentTypeProto.Stock)
        {
            return cqrsInvoker.Command(new StockChangePrice
            {
                InstrumentId = @event.InstrumentId,
                NewPrice = @event.NewPrice,
            });
        }
        
        return Task.CompletedTask;
    }
}