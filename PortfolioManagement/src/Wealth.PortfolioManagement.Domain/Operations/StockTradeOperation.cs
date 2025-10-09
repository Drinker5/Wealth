using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed record StockTradeOperation : TradeOperation
{
    public StockId StockId { get; set; }
}