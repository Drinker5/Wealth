using Google.Protobuf.WellKnownTypes;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class BondPriceChangedEventHandler(
    IKafkaProducer<InstrumentPriceChangedIntegrationEvent> producer) : IDomainEventHandler<BondPriceChanged>
{
    public Task Handle(BondPriceChanged notification, CancellationToken cancellationToken)
    {
        return producer.Produce(
            new InstrumentPriceChangedIntegrationEvent
            {
                OccuredOn = notification.OccurredOn.ToTimestamp(),
                InstrumentId = notification.BondId.Value,
                InstrumentType = InstrumentTypeProto.Bond,
                NewPrice = notification.NewPrice,
            },
            cancellationToken);
    }
}