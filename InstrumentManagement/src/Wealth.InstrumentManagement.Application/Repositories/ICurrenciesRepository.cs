using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Repositories;

public interface ICurrenciesRepository
{
    Task<IReadOnlyCollection<Currency>> GetCurrencies();
    Task<Currency?> GetCurrency(CurrencyId id);
    Task<Currency?> GetCurrency(FIGI figi);
    Task<Currency?> GetCurrency(InstrumentUId uId);
    Task DeleteCurrency(CurrencyId id);
    Task ChangePrice(CurrencyId id, Money price);
    Task<CurrencyId> CreateCurrency(CreateCurrencyCommand command, CancellationToken token);
    Task<CurrencyId> UpsertCurrency(CreateCurrencyCommand command, CancellationToken token);
}