using Eventso.Subscription;

namespace Wealth.PortfolioManagement.API.Services;

public sealed class OperationDeserializer : IMessageDeserializer
{
    public ConsumedMessage Deserialize<TContext>(ReadOnlySpan<byte> message, in TContext context) where TContext : IDeserializationContext
    {
        if (message.Length == 0)
            return ConsumedMessage.Skipped;

        var proto = Tinkoff.InvestApi.V1.Operation.Parser.ParseFrom(message);
        return new ConsumedMessage(proto);
    }
}