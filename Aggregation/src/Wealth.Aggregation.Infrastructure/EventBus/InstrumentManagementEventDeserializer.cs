using Eventso.Subscription;
using Wealth.InstrumentManagement;

namespace Wealth.Aggregation.Infrastructure.EventBus;

public sealed class InstrumentManagementEventDeserializer : IMessageDeserializer
{
    public ConsumedMessage Deserialize<TContext>(ReadOnlySpan<byte> message, in TContext context) where TContext : IDeserializationContext
    {
        if (message.Length == 0)
            return ConsumedMessage.Skipped;

        var proto = InstrumentManagementEvent.Parser.ParseFrom(message);
        return new ConsumedMessage(proto);
    }
}