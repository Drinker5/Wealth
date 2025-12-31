using System.Collections.Concurrent;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public sealed class InstrumentIdProviderCacheDecorator(IInstrumentIdProvider instrumentIdProvider) : IInstrumentIdProvider
{
    private readonly ConcurrentDictionary<InstrumentUId, int> cache = new();

    public async ValueTask<StockId> GetStockId(InstrumentUId instrumentUId, CancellationToken token)
    {
        if (cache.TryGetValue(instrumentUId, out var stockId))
            return stockId;

        stockId = await instrumentIdProvider.GetStockId(instrumentUId, token);
        cache.AddOrUpdate(instrumentUId, stockId, (_, _) => stockId);
        return stockId;
    }

    public async ValueTask<BondId> GetBondId(InstrumentUId instrumentUId, CancellationToken token)
    {
        if (cache.TryGetValue(instrumentUId, out var bondId))
            return bondId;

        bondId = await instrumentIdProvider.GetBondId(instrumentUId, token);
        cache.AddOrUpdate(instrumentUId, bondId, (_, _) => bondId);
        return bondId;
    }

    public async ValueTask<CurrencyId> GetCurrencyId(InstrumentUId instrumentUId, CancellationToken token)
    {
        if (cache.TryGetValue(instrumentUId, out var currencyId))
            return currencyId;

        currencyId = await instrumentIdProvider.GetCurrencyId(instrumentUId, token);
        cache.AddOrUpdate(instrumentUId, currencyId, (_, _) => currencyId);
        return currencyId;
    }

    public async Task<IReadOnlyDictionary<InstrumentUId, int>> GetInstruments(
        IReadOnlySet<InstrumentUIdType> instrumentIdTypes,
        CancellationToken token)
    {
        var toResolve = new HashSet<InstrumentUIdType>();
        foreach (var instrumentIdType in instrumentIdTypes)
        {
            if (!cache.ContainsKey(instrumentIdType.UId))
                toResolve.Add(instrumentIdType);
        }

        if (toResolve.Count == 0)
            return cache;

        var resolved = await instrumentIdProvider.GetInstruments(toResolve, token);

        foreach (var instrument in resolved)
            cache.AddOrUpdate(instrument.Key, instrument.Value, (_, _) => instrument.Value);

        return cache;
    }
}