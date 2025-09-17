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

    public async ValueTask<StockId> GetStockIdByFigi(string figi)
    {
        var share = await client.Instruments.ShareByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Figi,
            Id = figi
        });

        var stock = await instrumentsServiceClient.GetStockAsync(new GetStockRequest
        {
            Isin = share.Instrument.Isin
        });

        return stock.StockId;
    }

    public async ValueTask<BondId> GetBondIdByFigi(string figi)
    {
        var share = await client.Instruments.ShareByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Figi,
            Id = figi
        });

        var bond = await instrumentsServiceClient.GetBondAsync(new GetBondRequest
        {
            Isin = share.Instrument.Isin
        });

        return bond.BondId;
    }
}