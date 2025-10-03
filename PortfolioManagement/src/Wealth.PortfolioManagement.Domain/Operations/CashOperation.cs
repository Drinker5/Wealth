using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Operations;

public class CashOperation : Operation
{
    public PortfolioId PortfolioId { get; set; }
    public Money Money { get; set; }
    public CashOperationType Type { get; set; }
}

public enum CashOperationType : byte
{
    Amortization,
    Tax
}