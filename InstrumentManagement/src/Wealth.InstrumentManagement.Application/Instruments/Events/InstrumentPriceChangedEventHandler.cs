using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Outbox;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class InstrumentPriceChangedEventHandler : IDomainEventHandler<InstrumentPriceChanged>
{
    private readonly IOutboxRepository outboxRepository;

    public InstrumentPriceChangedEventHandler(IOutboxRepository outboxRepository)
    {
        this.outboxRepository = outboxRepository;
    }
    public async Task Handle(InstrumentPriceChanged notification, CancellationToken cancellationToken)
    {
        await outboxRepository.Add(notification);
    }
}
