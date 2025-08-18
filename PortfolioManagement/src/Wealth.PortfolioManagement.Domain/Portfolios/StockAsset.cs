using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public class StockAsset
{
    public StockId StockId { get; set; }
    public int Quantity { get; set; }
}