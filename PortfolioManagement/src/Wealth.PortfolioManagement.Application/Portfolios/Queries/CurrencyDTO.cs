using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class CurrencyDTO
{
    public string CurrencyId { get; set; }

    public decimal Amount { get; set; }

    public static CurrencyDTO ToDTO(PortfolioCurrency currency)
    {
        return new CurrencyDTO
        {
            CurrencyId = currency.CurrencyId.ToString(),
            Amount = currency.Amount,
        };
    }
}