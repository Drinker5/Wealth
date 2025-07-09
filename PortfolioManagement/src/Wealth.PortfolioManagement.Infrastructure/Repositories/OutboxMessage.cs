namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; }
    public string Data { get; init; }
    public DateTimeOffset OccurredOn { get; init; }
    public DateTimeOffset ProcessedOn { get; init; } = default;
}