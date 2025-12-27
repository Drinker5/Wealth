using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public interface IInstrumentIdProvider
{
    ValueTask<StockId> GetStockId(InstrumentId instrumentId, CancellationToken token);

    ValueTask<BondId> GetBondId(InstrumentId instrumentId, CancellationToken token);

    ValueTask<CurrencyId> GetCurrencyId(InstrumentId instrumentId, CancellationToken token);

    Task<IReadOnlyDictionary<InstrumentId, int>> GetInstruments(
        IReadOnlySet<InstrumentIdType> instrumentIdTypes,
        CancellationToken token);
}