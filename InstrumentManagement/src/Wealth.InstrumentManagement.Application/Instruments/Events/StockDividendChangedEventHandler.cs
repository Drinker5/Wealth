using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class StockDividendChangedEventHandler(IOutboxRepository outboxRepository) : IDomainEventHandler<StockDividendChanged>
{
    public async Task Handle(StockDividendChanged notification, CancellationToken cancellationToken)
    {
        await outboxRepository.Add(notification, cancellationToken);
    }
}
