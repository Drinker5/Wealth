using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class BondCouponChangedEventHandler(IOutboxRepository outboxRepository) : IDomainEventHandler<BondCouponChanged>
{
    public async Task Handle(BondCouponChanged notification, CancellationToken cancellationToken)
    {
        await outboxRepository.Add(
            notification,
            new BondCouponChangedIntegrationEvent
            {
                InstrumentId = notification.BondId,
                ISIN = notification.ISIN,
                NewCoupon = notification.NewCoupon.ValuePerYear,
            },
            notification.BondId.ToString(),
            cancellationToken);
    }
}