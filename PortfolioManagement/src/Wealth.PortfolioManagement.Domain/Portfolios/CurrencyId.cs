namespace Wealth.PortfolioManagement.Domain.Portfolios;

public record CurrencyId(string Code)
{
    public static implicit operator string(CurrencyId id)
    {
        return id.Code;
    }
    
    public static implicit operator CurrencyId(string code)
    {
        return new CurrencyId(code);
    }
    
    public override string ToString()
    {
        return Code;
    }
}