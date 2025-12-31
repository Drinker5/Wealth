using Grpc.Core;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;
using InstrumentsService = Wealth.InstrumentManagement.InstrumentsService;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public sealed class InstrumentIdProvider(
    InstrumentsService.InstrumentsServiceClient instrumentsServiceClient) : IInstrumentIdProvider
{
    public async ValueTask<StockId> GetStockId(InstrumentUId instrumentUId, CancellationToken token)
    {
        var stock = await GetStock(instrumentUId, token);
        if (stock != null)
            return stock.Value;

        var response = await CreateInstrument(stockInstrumentIds: [instrumentUId], token: token);
        return new StockId(response.Single().Id);
    }

    public async ValueTask<BondId> GetBondId(InstrumentUId instrumentUId, CancellationToken token)
    {
        var bond = await GetBond(instrumentUId, token);
        if (bond != null)
            return bond.Value;

        var response = await CreateInstrument(bondInstrumentIds: [instrumentUId], token: token);
        return new BondId(response.Single().Id);
    }

    public async ValueTask<CurrencyId> GetCurrencyId(InstrumentUId instrumentUId, CancellationToken token)
    {
        var currency = await GetCurrency(instrumentUId, token);
        if (currency != null)
            return currency.Value;

        var response = await CreateInstrument(currencyInstrumentIds: [instrumentUId], token: token);
        return new CurrencyId(response.Single().Id);
    }

    public async Task<IReadOnlyDictionary<InstrumentUId, int>> GetInstruments(
        IReadOnlySet<InstrumentUIdType> instrumentIdTypes,
        CancellationToken token)
    {
        var response = await instrumentsServiceClient.GetInstrumentsAsync(new GetInstrumentsRequest
        {
            InstrumentIds = { instrumentIdTypes.Select(i => new InstrumentIdProto(i.UId)) }
        }, cancellationToken: token);

        var responseDict = response.Instruments.ToDictionary(i => new InstrumentUIdType(i.InstrumentId, i.Type.FromProto()));

        var toCreate = new List<InstrumentUIdType>();
        foreach (var instrumentIdType in instrumentIdTypes.Except(responseDict.Keys))
            toCreate.Add(instrumentIdType);

        if (toCreate.Count > 0)
            await CreateInstruments(toCreate, token);

        return response.Instruments.ToDictionary(i => new InstrumentUId(i.InstrumentId.Value), i => i.Id);
    }

    private async Task<StockId?> GetStock(InstrumentUId instrumentUId, CancellationToken token)
    {
        try
        {
            var response = await instrumentsServiceClient.GetStockAsync(new GetStockRequest
            {
                InstrumentId = instrumentUId
            }, cancellationToken: token);

            return response.StockInfo.StockId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<BondId?> GetBond(InstrumentUId instrumentUId, CancellationToken token)
    {
        try
        {
            var getBondResponse = await instrumentsServiceClient.GetBondAsync(new GetBondRequest
            {
                InstrumentId = instrumentUId
            }, cancellationToken: token);
            return getBondResponse.BondId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<CurrencyId?> GetCurrency(InstrumentUId instrumentUId, CancellationToken token)
    {
        try
        {
            var getCurrencyResponse = await instrumentsServiceClient.GetCurrencyAsync(new GetCurrencyRequest
            {
                InstrumentId = instrumentUId
            }, cancellationToken: token);
            return getCurrencyResponse.CurrencyId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<IReadOnlyCollection<InstrumentProto>> CreateInstrument(
        IEnumerable<InstrumentUId>? stockInstrumentIds = null,
        IEnumerable<InstrumentUId>? bondInstrumentIds = null,
        IEnumerable<InstrumentUId>? currencyInstrumentIds = null,
        CancellationToken token = default)
    {
        var response = await instrumentsServiceClient.ImportInstrumentsAsync(new ImportInstrumentsRequest
        {
            StockInstrumentIds = { stockInstrumentIds?.Select(i => new InstrumentIdProto(i.Value)) },
            BondInstrumentIds = { bondInstrumentIds?.Select(i => new InstrumentIdProto(i.Value)) },
            CurrencyInstrumentIds = { currencyInstrumentIds?.Select(i => new InstrumentIdProto(i.Value)) }
        }, cancellationToken: token);

        return response.Instruments;
    }

    private Task<IReadOnlyCollection<InstrumentProto>> CreateInstruments(List<InstrumentUIdType> toCreate, CancellationToken token) =>
        CreateInstrument(
            stockInstrumentIds: toCreate.Where(i => i.Type == InstrumentType.Stock).Select(i => i.UId),
            bondInstrumentIds: toCreate.Where(i => i.Type == InstrumentType.Bond).Select(i => i.UId),
            currencyInstrumentIds: toCreate.Where(i => i.Type == InstrumentType.CurrencyAsset).Select(i => i.UId),
            token);
}