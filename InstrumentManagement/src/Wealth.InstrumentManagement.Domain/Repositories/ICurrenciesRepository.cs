using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Domain.Repositories;

public interface ICurrenciesRepository
{
    Task<IReadOnlyCollection<Currency>> GetCurrencies();
    Task<Currency?> GetCurrency(CurrencyId id);
    Task<Currency?> GetCurrency(FIGI figi);
    Task DeleteCurrency(CurrencyId id);
    Task ChangePrice(CurrencyId id, Money price);
    Task<CurrencyId> CreateCurrency(string name, FIGI figi, CancellationToken token);
}