using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public record CurrencyBrokerFeeOperation : Operation
{
    public PortfolioId PortfolioId { get; set; }
    public CurrencyId CurrencyId { get; set; }
    public Money Amount { get; set; }
}