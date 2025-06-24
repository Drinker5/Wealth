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

    public async Task<DateOnly> GetLastExchangeRateDate(CurrencyId requestFromCurrency, CurrencyId requestToCurrency)
    {
        var date = await context.ExchangeRates.AsNoTracking()
            .Where(e => e.BaseCurrencyId == requestFromCurrency && e.TargetCurrencyId == requestToCurrency)
            .OrderByDescending(e => e.ValidOnDate)
            .Select(e => e.ValidOnDate)
            .FirstOrDefaultAsync();

        if (date == default)
            return new DateOnly(2018, 12, 31);

        return date;
    }

    public async Task<PaginatedResult<ExchangeRate>> GetExchangeRates(CurrencyId requestFromId, CurrencyId requestToId, PageRequest pageRequest)
    {
        var skip = pageRequest.PageSize * (pageRequest.Page - 1);
        var take = pageRequest.PageSize;
        var queryable = context.ExchangeRates.AsNoTracking()
            .Where(e => e.BaseCurrencyId == requestFromId)
            .Where(e => e.TargetCurrencyId == requestToId);
        var total = await queryable.CountAsync();
        var exchangeRates = await queryable
            .OrderByDescending(e => e.ValidOnDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PaginatedResult<ExchangeRate>(exchangeRates.AsReadOnly(), total);
    }
}