using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public class CurrencyAsset
{
    public CurrencyId CurrencyId { get; set; }
    public int Quantity { get; set; }
}