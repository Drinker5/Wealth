using Eventso.Subscription;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Infrastructure.EventBus;

public sealed class StockPriceChangedIntegrationEventDeserializer : IMessageDeserializer
{
    public ConsumedMessage Deserialize<TContext>(ReadOnlySpan<byte> message, in TContext context) where TContext : IDeserializationContext
    {
        if (message.Length == 0)
            return ConsumedMessage.Skipped;

        var proto = StockPriceChangedIntegrationEvent.Parser.ParseFrom(message);
        return new ConsumedMessage(proto);
    }
}