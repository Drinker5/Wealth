using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public sealed record MoneyOperation : Operation
{
    public PortfolioId PortfolioId { get; set; }
    public Money Amount { get; set; }
    public MoneyOperationType Type { get; set; }
}

public enum MoneyOperationType : byte
{
    Deposit,
    Withdraw
}
