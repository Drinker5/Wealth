using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed class StockBoughtOperation : TradeOperation
{
    public StockId StockId { get; set; }
}