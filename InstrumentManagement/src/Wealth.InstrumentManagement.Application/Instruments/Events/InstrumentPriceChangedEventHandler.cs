using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class InstrumentPriceChangedEventHandler(IOutboxRepository outboxRepository) : IDomainEventHandler<InstrumentPriceChanged>
{
    public async Task Handle(InstrumentPriceChanged notification, CancellationToken cancellationToken)
    {
        await outboxRepository.Add(notification, cancellationToken);
    }
}
