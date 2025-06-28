using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class InstrumentPriceChangedEventHandler : IDomainEventHandler<InstrumentPriceChanged>
{
    public Task Handle(InstrumentPriceChanged notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
