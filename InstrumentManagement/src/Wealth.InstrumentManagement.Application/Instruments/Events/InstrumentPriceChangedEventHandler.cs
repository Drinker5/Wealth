using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class InstrumentPriceChangedEventHandler(IOutboxRepository outboxRepository) : IDomainEventHandler<InstrumentPriceChanged>
{
    public async Task Handle(InstrumentPriceChanged notification, CancellationToken cancellationToken)
    {
        await outboxRepository.Add(
            IntegrationEvent.Create(
                new InstrumentPriceChangedIntegrationEvent
                {
                    InstrumentId = notification.Id,
                    ISIN = notification.ISIN,
                    NewPrice = notification.NewPrice,
                },
                notification.Id.ToString()),
            cancellationToken);
    }
}