using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class StockDividendChangedEventHandler : IDomainEventHandler<StockDividendChanged>
{
    public Task Handle(StockDividendChanged notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
