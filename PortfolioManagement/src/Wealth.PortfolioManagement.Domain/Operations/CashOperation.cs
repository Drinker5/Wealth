using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Domain.Operations;

public class CashOperation : InstrumentOperation
{
    public PortfolioId PortfolioId { get; set; }
    public Money Money { get; set; }
    public CashOperationType Type { get; set; }
}

public enum CashOperationType
{
    Coupon,
    Dividend,
    Amortization,
    Tax
}