using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed class BondTradeOperation : TradeOperation
{
    public BondId BondId { get; set; }
}