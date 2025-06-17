using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Infrastructure.Repositories;

public class InMemoryCurrencyRepository : ICurrencyRepository
{
    private readonly List<Currency> currencies = [];

    public Task<Currency?> GetCurrency(CurrencyId id)
    {
        return Task.FromResult(currencies.SingleOrDefault(i => i.Id == id));
    }

    public async Task<Currency> CreateCurrency(CurrencyId id, string name, string symbol)
    {
        var exists = await GetCurrency(id);
        if (exists != null)
            return exists;
        
        var newCurrency = Currency.Create(id, name, symbol);
        currencies.Add(newCurrency);
        return newCurrency;
    }

    public async Task ChangeCurrencyName(CurrencyId id, string newName)
    {
        var exists = await GetCurrency(id);
        if (exists == null)
            return;
        
        exists.Rename(newName);
    }

    public Task<IEnumerable<Currency>> GetCurrencies()
    {
        return Task.FromResult(currencies.AsEnumerable());
    }
}