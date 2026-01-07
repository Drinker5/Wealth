using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Options;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class BondPriceChangedEventHandler(
    IKafkaProducer producer,
    IOptions<InstrumentsProducerOptions> producerOptions) : IDomainEventHandler<BondPriceChanged>
{
    public async Task Handle(BondPriceChanged notification, CancellationToken cancellationToken)
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
                        InstrumentId = notification.BondId.Value,
                        InstrumentType = InstrumentTypeProto.Bond,
                        NewPrice = notification.NewPrice,
                    },
                },
            },
            cancellationToken);
    }
}