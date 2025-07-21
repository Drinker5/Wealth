using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class InstrumentPriceChangedEventHandler(IOutboxRepository outboxRepository) : IDomainEventHandler<InstrumentPriceChanged>
{
    public async Task Handle(InstrumentPriceChanged notification, CancellationToken cancellationToken)
    {
        await outboxRepository.Add(
            notification,
            new InstrumentPriceChangedIntegrationEvent
            {
                InstrumentId = notification.InstrumentId,
                ISIN = notification.ISIN,
                NewPrice = notification.NewPrice,
            },
            notification.InstrumentId.ToString(),
            cancellationToken);
    }
}