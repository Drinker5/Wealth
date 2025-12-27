using Grpc.Core;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;
using InstrumentsService = Wealth.InstrumentManagement.InstrumentsService;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public sealed class InstrumentIdProvider(
    InstrumentsService.InstrumentsServiceClient instrumentsServiceClient) : IInstrumentIdProvider
{
    public async ValueTask<StockId> GetStockId(InstrumentId instrumentId, CancellationToken token)
    {
        var stock = await GetStock(instrumentId, token);
        if (stock != null)
            return stock.Value;

        var response = await CreateInstrument(stockInstrumentIds: [instrumentId], token: token);
        return new StockId(response.Id);
    }

    public async ValueTask<BondId> GetBondId(InstrumentId instrumentId, CancellationToken token)
    {
        var bond = await GetBond(instrumentId, token);
        if (bond != null)
            return bond.Value;

        var response = await CreateInstrument(bondInstrumentIds: [instrumentId], token: token);
        return new BondId(response.Id);
    }

    public async ValueTask<CurrencyId> GetCurrencyId(InstrumentId instrumentId, CancellationToken token)
    {
        var currency = await GetCurrency(instrumentId, token);
        if (currency != null)
            return currency.Value;

        var response = await CreateInstrument(currencyInstrumentIds: [instrumentId], token: token);
        return new CurrencyId(response.Id);
    }

    public async Task<IReadOnlyDictionary<InstrumentId, int>> GetInstruments(
        IReadOnlySet<InstrumentIdType> instrumentIdTypes,
        CancellationToken token)
    {
        var response = await instrumentsServiceClient.GetInstrumentsAsync(new GetInstrumentsRequest
        {
            InstrumentIds = { instrumentIdTypes.Select(i => new InstrumentIdProto(i.Id)) }
        }, cancellationToken: token);

        var responseDict = response.Instruments.ToDictionary(i => new InstrumentIdType(i.InstrumentId, i.Type.FromProto()));

        var toCreate = new List<InstrumentIdType>();
        foreach (var instrumentIdType in instrumentIdTypes.Except(responseDict.Keys))
            toCreate.Add(instrumentIdType);

        if (toCreate.Count > 0)
            await CreateInstruments(toCreate, token);

        return response.Instruments.ToDictionary(i => new InstrumentId(i.InstrumentId.Value), i => i.Id);
    }

    private async Task<StockId?> GetStock(InstrumentId instrumentId, CancellationToken token)
    {
        try
        {
            var response = await instrumentsServiceClient.GetStockAsync(new GetStockRequest
            {
                InstrumentId = instrumentId
            }, cancellationToken: token);

            return response.StockInfo.StockId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<BondId?> GetBond(InstrumentId instrumentId, CancellationToken token)
    {
        try
        {
            var getBondResponse = await instrumentsServiceClient.GetBondAsync(new GetBondRequest
            {
                InstrumentId = instrumentId
            }, cancellationToken: token);
            return getBondResponse.BondId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<CurrencyId?> GetCurrency(InstrumentId instrumentId, CancellationToken token)
    {
        try
        {
            var getCurrencyResponse = await instrumentsServiceClient.GetCurrencyAsync(new GetCurrencyRequest
            {
                InstrumentId = instrumentId
            }, cancellationToken: token);
            return getCurrencyResponse.CurrencyId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<InstrumentProto> CreateInstrument(
        IEnumerable<InstrumentId>? stockInstrumentIds = null,
        IEnumerable<InstrumentId>? bondInstrumentIds = null,
        IEnumerable<InstrumentId>? currencyInstrumentIds = null,
        CancellationToken token = default)
    {
        var response = await instrumentsServiceClient.ImportInstrumentsAsync(new ImportInstrumentsRequest
        {
            StockInstrumentIds = { stockInstrumentIds?.Select(i => new InstrumentIdProto(i.Value)) },
            BondInstrumentIds = { bondInstrumentIds?.Select(i => new InstrumentIdProto(i.Value)) },
            CurrencyInstrumentIds = { currencyInstrumentIds?.Select(i => new InstrumentIdProto(i.Value)) }
        }, cancellationToken: token);

        return response.Instruments.Single();
    }

    private Task<InstrumentProto> CreateInstruments(List<InstrumentIdType> toCreate, CancellationToken token) =>
        CreateInstrument(
            stockInstrumentIds: toCreate.Where(i => i.Type == InstrumentType.Stock).Select(i => i.Id),
            bondInstrumentIds: toCreate.Where(i => i.Type == InstrumentType.Bond).Select(i => i.Id),
            currencyInstrumentIds: toCreate.Where(i => i.Type == InstrumentType.CurrencyAsset).Select(i => i.Id),
            token);
}