using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed record BondBrokerFeeOperation : Operation
{
    public PortfolioId PortfolioId { get; set; }
    public BondId BondId { get; set; }
    public Money Amount { get; set; }
}