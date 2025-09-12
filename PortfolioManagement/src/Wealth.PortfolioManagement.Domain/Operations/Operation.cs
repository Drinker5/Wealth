namespace Wealth.PortfolioManagement.Domain.Operations;

public abstract class Operation
{
    public string Id { get; set; }
    public DateTimeOffset Date { get; set; }
}