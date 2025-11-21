using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class CurrencyDTO
{
    public CurrencyCode Currency { get; set; }

    public decimal Amount { get; set; }

    public static CurrencyDTO ToDTO(PortfolioCurrency currency)
    {
        return new CurrencyDTO
        {
            Currency = currency.Currency,
            Amount = currency.Amount,
        };
    }
}