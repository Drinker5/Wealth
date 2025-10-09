using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed record CurrencyOperation : Operation
{
    public PortfolioId PortfolioId { get; set; }
    public Money Amount { get; set; }
    public CurrencyOperationType Type { get; set; }
}

public enum CurrencyOperationType : byte
{
    Deposit,
    Withdraw
}
