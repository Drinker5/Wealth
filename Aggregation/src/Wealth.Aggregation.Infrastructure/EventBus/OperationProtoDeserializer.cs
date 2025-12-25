using Eventso.Subscription;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Infrastructure.EventBus;

public sealed class OperationProtoDeserializer : IMessageDeserializer
{
    public ConsumedMessage Deserialize<TContext>(ReadOnlySpan<byte> message, in TContext context) where TContext : IDeserializationContext
    {
        if (message.Length == 0)
            return ConsumedMessage.Skipped;

        var proto = OperationProto.Parser.ParseFrom(message);
        return new ConsumedMessage(proto);
    }
}