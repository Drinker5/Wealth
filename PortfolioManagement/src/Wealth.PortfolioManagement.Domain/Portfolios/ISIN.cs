namespace Wealth.PortfolioManagement.Domain.Portfolios;

public record ISIN(string Code)
{
    public static implicit operator string(ISIN isin)
    {
        return isin.Code;
    }

    public static implicit operator ISIN(string code)
    {
        return new ISIN(code);
    }
}