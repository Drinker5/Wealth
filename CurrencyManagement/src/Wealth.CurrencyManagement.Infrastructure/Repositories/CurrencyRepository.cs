using Microsoft.EntityFrameworkCore;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.Repositories;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

namespace Wealth.CurrencyManagement.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly WealthDbContext context;

    public CurrencyRepository(WealthDbContext context)
    {
        this.context = context;
    }

    public Task<Currency?> GetCurrency(CurrencyId id)
    {
        return context.Currencies.SingleOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Currency> CreateCurrency(CurrencyId id, string name, string symbol)
    {
        var exists = await GetCurrency(id);
        if (exists != null)
            return exists;

        var newCurrency = Currency.Create(id, name, symbol);
        await context.Currencies.AddAsync(newCurrency);
        return newCurrency;
    }

    public async Task ChangeCurrencyName(CurrencyId id, string newName)
    {
        var exists = await GetCurrency(id);
        if (exists == null)
            return;

        exists.Rename(newName);
    }

    public async Task<IEnumerable<Currency>> GetCurrencies()
    {
        return await context.Currencies.AsNoTracking().ToListAsync();
    }

    public async Task DeleteCurrency(CurrencyId requestCurrencyId)
    {
        var currency = await GetCurrency(requestCurrencyId);
        if (currency == null)
            return;

        context.Currencies.Remove(currency);
    }
}