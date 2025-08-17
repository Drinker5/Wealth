namespace Wealth.PortfolioManagement.Domain.Operations;

public abstract class InstrumentOperation
{
    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
}