namespace Wealth.PortfolioManagement.Domain.Portfolios;

public record struct PortfolioId(int Id)
{
    public static PortfolioId New() => new(0);

    public static implicit operator int(PortfolioId id)
    {
        return id.Id;
    }

    public static implicit operator PortfolioId(int id)
    {
        return new PortfolioId(id);
    }

    public readonly override int GetHashCode()
    {
        return Id;
    }
}
