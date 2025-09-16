using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public class InstrumentIdProvider(IOptions<TBankOperationProviderOptions> options) : IInstrumentIdProvider
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);

    public async ValueTask<StockId> GetStockIdByFigi(string figi)
    {
        var share = await client.Instruments.ShareByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Figi,
            Id = figi
        });

        // share.Instrument.Isin
        throw new NotImplementedException();
    }
    public ValueTask<BondId> GetBondIdByFigi(string figi)
    {
        throw new NotImplementedException();
    }
}