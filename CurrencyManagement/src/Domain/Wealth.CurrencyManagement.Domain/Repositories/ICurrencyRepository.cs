using Wealth.CurrencyManagement.Domain.Currency;

namespace Wealth.CurrencyManagement.Domain.Repositories;

public interface ICurrencyRepository
{
    Task<Currency.Currency?> GetCurrency(CurrencyId id);
    Task<Currency.Currency> CreateCurrency(CurrencyId id, string name, string symbol);
    Task ChangeCurrencyName(CurrencyId id, string newName);
}