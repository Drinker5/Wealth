using Microsoft.EntityFrameworkCore;
using Wealth.CurrencyManagement.Domain.Currency;
using Wealth.CurrencyManagement.Domain.Repositories;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWork;

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
        return context.Currencies.AsNoTracking().SingleOrDefaultAsync(i => i.Id == id);
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
}