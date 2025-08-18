using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public class CurrencyOperation : Operation
{
    public PortfolioId PortfolioId { get; set; }
    public Money Money { get; set; }
    public CurrencyOperationType Type { get; set; }
}

public enum CurrencyOperationType
{
    Deposit,
    Withdraw
}
