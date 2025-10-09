using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public record StockBrokerFeeOperation : Operation
{
    public PortfolioId PortfolioId { get; set; }
    public StockId StockId { get; set; }
    public Money Amount { get; set; }
}