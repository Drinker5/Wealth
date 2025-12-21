using Grpc.Core;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;
using InstrumentsService = Wealth.InstrumentManagement.InstrumentsService;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public sealed class InstrumentIdProvider(
    InstrumentsService.InstrumentsServiceClient instrumentsServiceClient) : IInstrumentIdProvider
{
    public async ValueTask<StockId> GetStockId(InstrumentId instrumentId)
    {
        var stock = await GetStock(instrumentId);
        if (stock != null)
            return stock.Value;

        var response = await CreateInstrument(stockInstrumentIds: [instrumentId]);
        return new StockId(response.Id);
    }

    public async ValueTask<BondId> GetBondId(InstrumentId instrumentId)
    {
        var bond = await GetBond(instrumentId);
        if (bond != null)
            return bond.Value;

        var response = await CreateInstrument(bondInstrumentIds: [instrumentId]);
        return new BondId(response.Id);
    }

    public async ValueTask<CurrencyId> GetCurrencyId(InstrumentId instrumentId)
    {
        var currency = await GetCurrency(instrumentId);
        if (currency != null)
            return currency.Value;

        var response = await CreateInstrument(currencyInstrumentIds: [instrumentId]);
        return new CurrencyId(response.Id);
    }

    private async Task<StockId?> GetStock(InstrumentId instrumentId)
    {
        try
        {
            var response = await instrumentsServiceClient.GetStockAsync(new GetStockRequest
            {
                InstrumentId = instrumentId
            });

            return response.StockInfo.StockId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<BondId?> GetBond(InstrumentId instrumentId)
    {
        try
        {
            var getBondResponse = await instrumentsServiceClient.GetBondAsync(new GetBondRequest
            {
                InstrumentId = instrumentId
            });
            return getBondResponse.BondId;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<CurrencyId?> GetCurrency(InstrumentId instrumentId)
    {
        try
        {
            var getCurrencyResponse = await instrumentsServiceClient.GetCurrencyAsync(new GetCurrencyRequest
            {
                InstrumentId = instrumentId
            });
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
        IEnumerable<InstrumentId>? currencyInstrumentIds = null)
    {
        var response = await instrumentsServiceClient.ImportInstrumentsAsync(new ImportInstrumentsRequest
        {
            StockInstrumentIds = { stockInstrumentIds?.Select(i => new InstrumentIdProto(i.Value)) },
            BondInstrumentIds = { bondInstrumentIds?.Select(i => new InstrumentIdProto(i.Value)) },
            CurrencyInstrumentIds = { currencyInstrumentIds?.Select(i => new InstrumentIdProto(i.Value)) }
        });

        return response.Instruments.Single();
    }
}