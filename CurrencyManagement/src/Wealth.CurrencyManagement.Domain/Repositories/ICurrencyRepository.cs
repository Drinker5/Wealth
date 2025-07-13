using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Domain.Repositories;

public interface ICurrencyRepository
{
    Task<Currency?> GetCurrency(CurrencyId id);
    Task<Currency> CreateCurrency(CurrencyId id, string name, string symbol);
    Task ChangeCurrencyName(CurrencyId id, string newName);
    Task<IEnumerable<Currency>> GetCurrencies();
    Task DeleteCurrency(CurrencyId requestCurrencyId);
}