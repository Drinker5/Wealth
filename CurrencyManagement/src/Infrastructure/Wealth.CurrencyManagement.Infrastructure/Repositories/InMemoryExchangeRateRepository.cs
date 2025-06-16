using Wealth.CurrencyManagement.Domain.Currency;
using Wealth.CurrencyManagement.Domain.ExchangeRate;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Infrastructure.Repositories;

public class InMemoryExchangeRateRepository : IExchangeRateRepository
{
    private readonly List<ExchangeRate> exchangeRates = [];

    public Task<ExchangeRate?> GetExchangeRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, DateTime validOnDate)
    {
        var exchangeRate = exchangeRates.SingleOrDefault(e =>
            e.BaseCurrencyId == baseCurrencyId
            && e.TargetCurrencyId == targetCurrencyId
            && e.ValidOnDate == validOnDate);
        return Task.FromResult(exchangeRate);
    }

    public async Task<ExchangeRate> CreateExchangeRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, decimal rate, DateTime validOnDate)
    {
        var exist = await GetExchangeRate(baseCurrencyId, targetCurrencyId, validOnDate);
        if (exist != null)
            return exist;
        
        var exchangeRate = ExchangeRate.Create(baseCurrencyId, targetCurrencyId, rate, validOnDate);
        exchangeRates.Add(exchangeRate);
        return exchangeRate;
    }
}