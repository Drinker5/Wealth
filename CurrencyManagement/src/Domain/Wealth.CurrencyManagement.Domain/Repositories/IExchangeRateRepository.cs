using Wealth.CurrencyManagement.Domain.Entities;

namespace Wealth.CurrencyManagement.Domain.Repositories;

public interface IExchangeRateRepository
{
    Task<ExchangeRate> GetExchangeRate(
        CurrencyId baseCurrencyId,
        CurrencyId targetCurrencyId,
        DateTime validOnDate);

    Task<ExchangeRate> CreateExchangeRate(
        CurrencyId baseCurrencyId,
        CurrencyId targetCurrencyId,
        decimal rate,
        DateTime validOnDate);
}
