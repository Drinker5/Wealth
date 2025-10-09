using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed record StockDelistOperation : Operation
{
    public StockId StockId { get; init; }
}