using Microsoft.EntityFrameworkCore;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;
using Wealth.CurrencyManagement.Domain.Repositories;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

namespace Wealth.CurrencyManagement.Infrastructure.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly WealthDbContext context;

    public ExchangeRateRepository(WealthDbContext context)
    {
        this.context = context;
    }
    public Task<ExchangeRate?> GetExchangeRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, DateOnly validOnDate)
    {
        return context.ExchangeRates.AsNoTracking().SingleOrDefaultAsync(e =>
            e.BaseCurrencyId == baseCurrencyId
            && e.TargetCurrencyId == targetCurrencyId
            && e.ValidOnDate == validOnDate);
    }

    public async Task<ExchangeRate> CreateExchangeRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, decimal rate, DateOnly validOnDate)
    {
        var exist = await GetExchangeRate(baseCurrencyId, targetCurrencyId, validOnDate);
        if (exist != null)
            return exist;
        
        var exchangeRate = ExchangeRate.Create(baseCurrencyId, targetCurrencyId, rate, validOnDate);
        await context.ExchangeRates.AddAsync(exchangeRate);
        return exchangeRate;
    }
}