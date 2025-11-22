using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public class PortfolioCurrency
{
    public CurrencyCode Currency { get; init; }
    public decimal Amount { get; set; }

    public override int GetHashCode()
    {
        return Currency.GetHashCode();
    }
}