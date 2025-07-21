using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.BuildingBlocks.Application;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public string Key { get; init; }
    public string Type { get; init; }
    public byte[] Data { get; init; }
    public DateTimeOffset OccurredOn { get; init; }
    public DateTimeOffset? ProcessedOn { get; init; }
}

public static class OutboxMessageExtensions
{
    public static OutboxMessage ToOutboxMessage(
        this DomainEvent domainEvent, 
        IMessage message,
        string? key = null)
    {
        return new OutboxMessage
        {
            Id = domainEvent.Id,
            Key = key ?? message.GetType().Name,
            Type = message.GetType().Name,
            OccurredOn = domainEvent.OccurredOn,
            Data = Any.Pack(message).ToByteArray(),
        };
    }
}