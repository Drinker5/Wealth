using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
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

    public Task<ExchangeRate?> GetExchangeRate(
        CurrencyCode baseCurrency,
        CurrencyCode targetCurrency,
        DateOnly validOnDate)
    {
        return context.ExchangeRates.AsNoTracking().SingleOrDefaultAsync(e =>
            e.BaseCurrency == baseCurrency
            && e.TargetCurrency == targetCurrency
            && e.ValidOnDate == validOnDate);
    }

    public async Task<ExchangeRate> CreateExchangeRate(
        CurrencyCode baseCurrency,
        CurrencyCode targetCurrency,
        decimal rate,
        DateOnly validOnDate)
    {
        var exist = await GetExchangeRate(baseCurrency, targetCurrency, validOnDate);
        if (exist != null)
            return exist;

        var exchangeRate = ExchangeRate.Create(baseCurrency, targetCurrency, rate, validOnDate);
        await context.ExchangeRates.AddAsync(exchangeRate);
        return exchangeRate;
    }

    public async Task<DateOnly> GetLastExchangeRateDate(
        CurrencyCode requestFromCurrency,
        CurrencyCode requestToCurrency)
    {
        var date = await context.ExchangeRates.AsNoTracking()
            .Where(e => e.BaseCurrency == requestFromCurrency && e.TargetCurrency == requestToCurrency)
            .OrderByDescending(e => e.ValidOnDate)
            .Select(e => e.ValidOnDate)
            .FirstOrDefaultAsync();

        if (date == default)
            return new DateOnly(2018, 12, 31);

        return date;
    }

    public async Task<PaginatedResult<ExchangeRate>> GetExchangeRates(
        CurrencyCode requestFrom,
        CurrencyCode requestTo,
        PageRequest pageRequest)
    {
        var skip = pageRequest.PageSize * (pageRequest.Page - 1);
        var take = pageRequest.PageSize;
        var queryable = context.ExchangeRates.AsNoTracking()
            .Where(e => e.BaseCurrency == requestFrom)
            .Where(e => e.TargetCurrency == requestTo);
        var total = await queryable.CountAsync();
        var exchangeRates = await queryable
            .OrderByDescending(e => e.ValidOnDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PaginatedResult<ExchangeRate>(exchangeRates.AsReadOnly(), total);
    }
}