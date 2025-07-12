using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Operations;

public class CurrencyOperation
{
    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public PortfolioId PortfolioId { get; set; }
    public Money Money { get; set; }
    public CurrencyOperationType Type { get; set; }
}

public enum CurrencyOperationType
{
    Deposit,
    Withdraw
}
