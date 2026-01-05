using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class InstrumentPriceChangedEventHandler(IOutboxRepository outboxRepository) : IDomainEventHandler<StockPriceChanged>
{
    public async Task Handle(StockPriceChanged notification, CancellationToken cancellationToken)
    {
        await outboxRepository.Add(
            notification,
            new InstrumentPriceChangedIntegrationEvent()
            {
                InstrumentId =  notification.StockId.Value,
                InstrumentType = InstrumentTypeProto.Stock,
                NewPrice = notification.NewPrice,
            },
            notification.StockId.ToString(),
            cancellationToken);
    }
}