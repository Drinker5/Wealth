using System.Collections.Concurrent;
using Grpc.Core;
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

    //TODO metrics, count of existed/from cache, count of created

    public async ValueTask<StockId> GetStockIdByFigi(string figi)
    {
        if (stockIdCache.TryGetValue(figi, out var stockId))
            return stockId;

        var stock = await GetStock(figi);
        if (stock == null)
        {
            var response = await CreateStock(figi);
            stockId = response.StockId;
        }
        else
        {
            stockId = stock.Value;
        }

        stockIdCache.AddOrUpdate(figi, stockId, (_, _) => stockId);
        return stockId;
    }

    public async ValueTask<BondId> GetBondIdByFigi(string figi)
    {
        if (bondIdCache.TryGetValue(figi, out var bondId))
            return bondId;

        var bond = await GetBondAsync(figi);
        if (bond == null)
        {
            var response = await CreateBond(figi);
            bondId = response.BondId;
        }
        else
        {
            bondId = bond.Value;
        }

        bondIdCache.AddOrUpdate(figi, bondId, (_, _) => bondId);
        return bondId;
    }

    private async Task<StockId?> GetStock(string figi)
    {
        try
        {
            var getStockResponse = await instrumentsServiceClient.GetStockAsync(new GetStockRequest
            {
                Figi = figi
            });

            return getStockResponse.StockId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<BondId?> GetBondAsync(string figi)
    {
        try
        {
            var getBondResponse = await instrumentsServiceClient.GetBondAsync(new GetBondRequest
            {
                Figi = figi
            });
            return getBondResponse.BondId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<CreateStockResponse> CreateStock(string figi)
    {
        var share = await client.Instruments.ShareByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Figi,
            Id = figi
        });

        var response = await instrumentsServiceClient.CreateStockAsync(new CreateStockRequest
        {
            Figi = figi,
            Isin = share.Instrument.Isin,
            LotSize = share.Instrument.Lot,
            Name = share.Instrument.Name,
        });

        return response;
    }

    private async Task<CreateBondResponse> CreateBond(string figi)
    {
        var share = await client.Instruments.BondByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Figi,
            Id = figi
        });

        var response = await instrumentsServiceClient.CreateBondAsync(new CreateBondRequest
        {
            Figi = figi,
            Isin = share.Instrument.Isin,
            Name = share.Instrument.Name,
        });

        return response;
    }
}