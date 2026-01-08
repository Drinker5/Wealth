using Google.Protobuf.WellKnownTypes;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class BondCouponChangedEventHandler(
    IKafkaProducer<BondCouponChangedIntegrationEvent> producer) : IDomainEventHandler<BondCouponChanged>
{
    public Task Handle(BondCouponChanged notification, CancellationToken cancellationToken)
    {
        return producer.Produce(
            new BondCouponChangedIntegrationEvent
            {
                BondId = notification.BondId,
                ISIN = notification.ISIN,
                NewCoupon = notification.NewCoupon.ValuePerYear,
                OccuredOn = notification.OccurredOn.ToTimestamp(),
            },
            cancellationToken);
    }
}