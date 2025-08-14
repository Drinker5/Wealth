using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public abstract class PortfolioAsset
{
    public ISIN ISIN { get; init; }
    public int Quantity { get; set; }
}

public class Bond : PortfolioAsset
{
    public BondId BondId { get; set; }
}

public class Stock : PortfolioAsset
{
    public StockId StockId { get; set; }
}