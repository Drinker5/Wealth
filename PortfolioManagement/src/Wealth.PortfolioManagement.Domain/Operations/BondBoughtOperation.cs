using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed class BondBoughtOperation : TradeOperation
{
    public BondId BondId { get; set; }
}