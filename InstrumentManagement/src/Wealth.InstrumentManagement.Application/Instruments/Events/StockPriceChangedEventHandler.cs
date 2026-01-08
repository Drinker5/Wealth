using Google.Protobuf.WellKnownTypes;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class StockPriceChangedEventHandler(
    IKafkaProducer<InstrumentPriceChangedIntegrationEvent> producer) : IDomainEventHandler<StockPriceChanged>
{
    public Task Handle(StockPriceChanged notification, CancellationToken cancellationToken)
    {
        return producer.Produce(
            new InstrumentPriceChangedIntegrationEvent
            {
                InstrumentId = notification.StockId.Value,
                InstrumentType = InstrumentTypeProto.Stock,
                NewPrice = notification.NewPrice,
                OccuredOn = notification.OccurredOn.ToTimestamp()
            },
            cancellationToken);
    }
}