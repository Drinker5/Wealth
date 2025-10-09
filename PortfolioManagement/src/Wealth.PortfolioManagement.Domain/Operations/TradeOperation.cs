using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public abstract record TradeOperation : Operation
{
    public PortfolioId PortfolioId { get; set; }
    public Money Amount { get; set; }
    public long Quantity { get; set; }
    public TradeOperationType Type { get; set; }
}

public enum TradeOperationType
{
    Buy,
    Sell
}