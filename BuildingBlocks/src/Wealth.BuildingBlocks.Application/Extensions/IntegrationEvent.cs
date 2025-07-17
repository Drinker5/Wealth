using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.BuildingBlocks.Application;

public partial class IntegrationEvent
{
    public static IntegrationEvent Create(IMessage message, string? key = null)
    {
        return new IntegrationEvent
        {
            Id = Guid.NewGuid(),
            Key = key ?? message.GetType().Name,
            Type = message.GetType().Name,
            OccuredOn = Timestamp.FromDateTimeOffset(Clock.Now),
            Event = Any.Pack(message)
        };
    }
    
    public static implicit operator OutboxMessage(IntegrationEvent message)
    {
        return ToOutboxMessage(message);
    }

    public OutboxMessage ToOutboxMessage()
    {
        return ToOutboxMessage(this);
    }
    
    public static OutboxMessage ToOutboxMessage(IntegrationEvent message)
    {
        return new OutboxMessage
        {
            Id = message.Id,
            Key = message.Key,
            OccurredOn = message.OccuredOn.ToDateTimeOffset(),
            Data = message.Event.ToByteArray(),
            Type = message.Type,
        };
    }
}