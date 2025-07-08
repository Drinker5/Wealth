namespace Wealth.PortfolioManagement.Domain.Portfolios;

public record PortfolioId(Guid Id)
{
    public static PortfolioId New() => new(Guid.NewGuid());
    
    public static implicit operator Guid(PortfolioId id)
    {
        return id.Id;
    }
    
    public static implicit operator PortfolioId(Guid id)
    {
        return new PortfolioId(id);
    }
}