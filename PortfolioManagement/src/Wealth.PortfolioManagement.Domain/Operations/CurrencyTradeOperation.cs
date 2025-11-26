using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed record CurrencyTradeOperation : TradeOperation
{
    public CurrencyId CurrencyId { get; init; }
}