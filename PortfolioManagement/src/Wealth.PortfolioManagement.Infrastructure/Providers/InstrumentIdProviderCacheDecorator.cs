using System.Collections.Concurrent;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public sealed class InstrumentIdProviderCacheDecorator(IInstrumentIdProvider instrumentIdProvider) : IInstrumentIdProvider
{
    private readonly ConcurrentDictionary<InstrumentId, int> cache = new();

    public async ValueTask<StockId> GetStockId(InstrumentId instrumentId, CancellationToken token)
    {
        if (cache.TryGetValue(instrumentId, out var stockId))
            return stockId;

        stockId = await instrumentIdProvider.GetStockId(instrumentId, token);
        cache.AddOrUpdate(instrumentId, stockId, (_, _) => stockId);
        return stockId;
    }

    public async ValueTask<BondId> GetBondId(InstrumentId instrumentId, CancellationToken token)
    {
        if (cache.TryGetValue(instrumentId, out var bondId))
            return bondId;

        bondId = await instrumentIdProvider.GetBondId(instrumentId, token);
        cache.AddOrUpdate(instrumentId, bondId, (_, _) => bondId);
        return bondId;
    }

    public async ValueTask<CurrencyId> GetCurrencyId(InstrumentId instrumentId, CancellationToken token)
    {
        if (cache.TryGetValue(instrumentId, out var currencyId))
            return currencyId;

        currencyId = await instrumentIdProvider.GetCurrencyId(instrumentId, token);
        cache.AddOrUpdate(instrumentId, currencyId, (_, _) => currencyId);
        return currencyId;
    }

    public async Task<IReadOnlyDictionary<InstrumentId, int>> GetInstruments(
        IReadOnlySet<InstrumentIdType> instrumentIdTypes,
        CancellationToken token)
    {
        var toResolve = new HashSet<InstrumentIdType>();
        foreach (var instrumentIdType in instrumentIdTypes)
        {
            if (!cache.ContainsKey(instrumentIdType.Id))
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