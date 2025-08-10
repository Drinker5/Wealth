using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class InstrumentPriceChangedEventHandler(IOutboxRepository outboxRepository) : IDomainEventHandler<StockPriceChanged>
{
    public async Task Handle(StockPriceChanged notification, CancellationToken cancellationToken)
    {
        await outboxRepository.Add(
            notification,
            new StockPriceChangedIntegrationEvent
            {
                StockId = notification.StockId,
                ISIN = notification.ISIN,
                NewPrice = notification.NewPrice,
            },
            notification.StockId.ToString(),
            cancellationToken);
    }
}