using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;
using InstrumentsService = Wealth.InstrumentManagement.InstrumentsService;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public class InstrumentIdProvider(
    InstrumentsService.InstrumentsServiceClient instrumentsServiceClient,
    IOptions<TBankOperationProviderOptions> options) : IInstrumentIdProvider
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);
    private readonly ConcurrentDictionary<string, StockId> stockIdCache = new();
    private readonly ConcurrentDictionary<string, BondId> bondIdCache = new();

    public async ValueTask<StockId> GetStockIdByFigi(string figi)
    {
        if (stockIdCache.TryGetValue(figi, out var stockId))
            return stockId;
        
        var share = await client.Instruments.ShareByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Figi,
            Id = figi
        });

        var stock = await instrumentsServiceClient.GetStockAsync(new GetStockRequest
        {
            Isin = share.Instrument.Isin
        });

        stockIdCache.AddOrUpdate(figi, stockId, (_, _) => stockId);
        return stock.StockId;
    }

    public async ValueTask<BondId> GetBondIdByFigi(string figi)
    {
        if (bondIdCache.TryGetValue(figi, out var bondId))
            return bondId;

        var share = await client.Instruments.ShareByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Figi,
            Id = figi
        });

        var bond = await instrumentsServiceClient.GetBondAsync(new GetBondRequest
        {
            Isin = share.Instrument.Isin
        });

        bondIdCache.AddOrUpdate(figi, bondId, (_, _) => bondId);
        return bond.BondId;
    }
}