using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public class StockDelistOperation : Operation
{
    public StockId StockId { get; set; }
}