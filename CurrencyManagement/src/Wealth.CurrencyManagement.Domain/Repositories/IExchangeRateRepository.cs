using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Domain.Repositories;

public interface IExchangeRateRepository
{
    Task<ExchangeRate?> GetExchangeRate(CurrencyCode baseCurrency,
        CurrencyCode targetCurrency,
        DateOnly validOnDate);

    Task<ExchangeRate> CreateExchangeRate(
        CurrencyCode baseCurrency,
        CurrencyCode targetCurrency,
        decimal rate,
        DateOnly validOnDate);

    Task<DateOnly> GetLastExchangeRateDate(CurrencyCode requestFromCurrency, CurrencyCode requestToCurrency);
    Task<PaginatedResult<ExchangeRate>> GetExchangeRates(
        CurrencyCode requestFrom,
        CurrencyCode requestTo,
        PageRequest pageRequest);
}