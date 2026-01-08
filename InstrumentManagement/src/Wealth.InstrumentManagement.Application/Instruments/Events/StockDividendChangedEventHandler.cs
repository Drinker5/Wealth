using Google.Protobuf.WellKnownTypes;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

internal class StockDividendChangedEventHandler(
    IKafkaProducer<StockDividendChangedIntegrationEvent> producer) : IDomainEventHandler<StockDividendChanged>
{
    public Task Handle(StockDividendChanged notification, CancellationToken cancellationToken)
    {
        return producer.Produce(
            new StockDividendChangedIntegrationEvent
            {
                StockId = notification.StockId,
                ISIN = notification.ISIN,
                NewDividend = notification.NewDividend.ValuePerYear,
                OccuredOn = notification.OccurredOn.ToTimestamp(),
            },
            cancellationToken);
    }
}