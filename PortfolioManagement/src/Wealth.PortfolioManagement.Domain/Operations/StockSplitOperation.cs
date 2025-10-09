using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed record StockSplitOperation : Operation
{
    public StockId StockId { get; set; }
    public SplitRatio Ratio { get; set; }
}