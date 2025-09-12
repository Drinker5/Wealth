namespace Wealth.PortfolioManagement.Domain.Operations;

public abstract class Operation
{
    public string Id { get; init; }
    public DateTimeOffset Date { get; init; }
}