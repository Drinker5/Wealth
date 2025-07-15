namespace Wealth.BuildingBlocks.Application;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public string Key { get; init; }
    public string Type { get; init; }
    public string Data { get; init; }
    public DateTimeOffset OccurredOn { get; init; }
    public DateTimeOffset? ProcessedOn { get; init; }
}