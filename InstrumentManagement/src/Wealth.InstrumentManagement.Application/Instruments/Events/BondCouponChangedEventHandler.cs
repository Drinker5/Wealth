using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class BondCouponChangedEventHandler : IDomainEventHandler<BondCouponChanged>
{
    public Task Handle(BondCouponChanged notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
