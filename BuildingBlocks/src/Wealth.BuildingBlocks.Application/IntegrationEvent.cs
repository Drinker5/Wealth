using System.Text.Json.Serialization;
using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.BuildingBlocks.Application;

public abstract record IntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTimeOffset CreationDate { get; init; } = Clock.Now;

    /// <summary>
    /// kafka topic's key
    /// </summary>
    [JsonIgnore]
    public virtual string Key => GetType().Name;

    [JsonIgnore]
    private string Type => GetType().Name;

    public OutboxMessage ToOutboxMessage(Func<IntegrationEvent, string> serializer)
    {
        return new OutboxMessage
        {
            Id = Id,
            Key = Key,
            OccurredOn = CreationDate,
            Data = serializer.Invoke(this),
            Type = Type,
        };
    }
}