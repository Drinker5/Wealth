using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public interface IInstrumentIdProvider
{
    ValueTask<StockId> GetStockId(InstrumentUId instrumentUId, CancellationToken token);

    ValueTask<BondId> GetBondId(InstrumentUId instrumentUId, CancellationToken token);

    ValueTask<CurrencyId> GetCurrencyId(InstrumentUId instrumentUId, CancellationToken token);

    Task<IReadOnlyDictionary<InstrumentUId, int>> GetInstruments(
        IReadOnlySet<InstrumentUIdType> instrumentIdTypes,
        CancellationToken token);
}