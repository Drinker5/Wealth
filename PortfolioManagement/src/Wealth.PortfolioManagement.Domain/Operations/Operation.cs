namespace Wealth.PortfolioManagement.Domain.Operations;

public abstract class Operation
{
    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
}