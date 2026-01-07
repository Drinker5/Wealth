using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Options;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class BondCouponChangedEventHandler(
    IKafkaProducer producer,
    IOptions<InstrumentsProducerOptions> producerOptions) : IDomainEventHandler<BondCouponChanged>
{
    public async Task Handle(BondCouponChanged notification, CancellationToken cancellationToken)
    {
        await producer.Produce(
            producerOptions.Value.Topic,
            new BusMessage<string, InstrumentManagementEvent>
            {
                Key = notification.ToString(),
                Value = new InstrumentManagementEvent
                {
                    Id = notification.ToString(),
                    Date = notification.OccurredOn.ToTimestamp(),
                    BondCouponChanged = new BondCouponChangedIntegrationEvent
                    {
                        BondId = notification.BondId,
                        ISIN = notification.ISIN,
                        NewCoupon = notification.NewCoupon.ValuePerYear,
                    },
                },
            },
            cancellationToken);
    }
}