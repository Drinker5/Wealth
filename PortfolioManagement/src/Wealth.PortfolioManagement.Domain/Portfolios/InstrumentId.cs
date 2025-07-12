namespace Wealth.PortfolioManagement.Domain.Portfolios;

public record InstrumentId(Guid Id)
{
    public static implicit operator Guid(InstrumentId id)
    {
        return id.Id;
    }

    public static implicit operator InstrumentId(Guid id)
    {
        return new InstrumentId(id);
    }
}