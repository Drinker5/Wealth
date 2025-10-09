using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed record BondTradeOperation : TradeOperation
{
    public BondId BondId { get; set; }
}