using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Options;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class StockPriceChangedEventHandler(
    IKafkaProducer producer,
    IOptions<InstrumentsProducerOptions> producerOptions) : IDomainEventHandler<StockPriceChanged>
{
    public async Task Handle(StockPriceChanged notification, CancellationToken cancellationToken)
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
                    InstrumentPriceChanged = new InstrumentPriceChangedIntegrationEvent
                    {
                        InstrumentId = notification.StockId.Value,
                        InstrumentType = InstrumentTypeProto.Stock,
                        NewPrice = notification.NewPrice,
                    },
                },
            },
            cancellationToken);
    }
}