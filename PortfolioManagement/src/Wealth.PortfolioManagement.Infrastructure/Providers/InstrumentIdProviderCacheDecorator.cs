using System.Collections.Concurrent;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public sealed class InstrumentIdProviderCacheDecorator(IInstrumentIdProvider instrumentIdProvider) : IInstrumentIdProvider
{
    private readonly ConcurrentDictionary<InstrumentId, StockId> stockIdCache = new();
    private readonly ConcurrentDictionary<InstrumentId, BondId> bondIdCache = new();
    private readonly ConcurrentDictionary<InstrumentId, CurrencyId> currencyIdCache = new();

    public async ValueTask<StockId> GetStockId(InstrumentId instrumentId)
    {
        if (stockIdCache.TryGetValue(instrumentId, out var stockId))
            return stockId;

        stockId = await instrumentIdProvider.GetStockId(instrumentId);
        stockIdCache.AddOrUpdate(instrumentId, stockId, (_, _) => stockId);
        return stockId;
    }

    public async ValueTask<BondId> GetBondId(InstrumentId instrumentId)
    {
        if (bondIdCache.TryGetValue(instrumentId, out var bondId))
            return bondId;

        bondId = await instrumentIdProvider.GetBondId(instrumentId);
        bondIdCache.AddOrUpdate(instrumentId, bondId, (_, _) => bondId);
        return bondId;
    }

    public async ValueTask<CurrencyId> GetCurrencyId(InstrumentId instrumentId)
    {
        if (currencyIdCache.TryGetValue(instrumentId, out var currencyId))
            return currencyId;

        currencyId = await instrumentIdProvider.GetCurrencyId(instrumentId);
        currencyIdCache.AddOrUpdate(instrumentId, currencyId, (_, _) => currencyId);
        return currencyId;
    }
}