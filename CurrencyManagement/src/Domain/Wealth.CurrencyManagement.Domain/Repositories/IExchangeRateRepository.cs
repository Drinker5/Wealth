using Wealth.CurrencyManagement.Domain.Currency;

namespace Wealth.CurrencyManagement.Domain.Repositories;

public interface IExchangeRateRepository
{
    Task<ExchangeRate.ExchangeRate?> GetExchangeRate(CurrencyId baseCurrencyId,
        CurrencyId targetCurrencyId,
        DateTime validOnDate);

    Task<ExchangeRate.ExchangeRate> CreateExchangeRate(
        CurrencyId baseCurrencyId,
        CurrencyId targetCurrencyId,
        decimal rate,
        DateTime validOnDate);
}
