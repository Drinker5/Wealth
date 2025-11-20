using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public class PortfolioCurrency
{
    public CurrencyCode CurrencyId { get; init; }
    public decimal Amount { get; set; }

    public override int GetHashCode()
    {
        return CurrencyId.GetHashCode();
    }
}