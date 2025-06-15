using Wealth.CurrencyManagement.Domain.Entities;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Infrastructure;

public class InMemoryCurrencyRepository : ICurrencyRepository
{
    public Task<Currency> GetCurrency(CurrencyId id)
    {
        throw new NotImplementedException();
    }

    public Task<Currency> CreateCurrency(CurrencyId id, string name, string symbol)
    {
        throw new NotImplementedException();
    }

    public Task ChangeCurrencyName(CurrencyId id, string newName)
    {
        throw new NotImplementedException();
    }
}