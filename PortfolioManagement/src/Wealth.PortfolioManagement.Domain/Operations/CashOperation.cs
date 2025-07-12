using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.ValueObjects;

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