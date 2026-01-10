using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public sealed class TBankInstrumentPricesProvider(
    IOptions<TBankInstrumentsProviderOptions> options) : IInstrumentPricesProvider
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);

    public async Task<IReadOnlyDictionary<InstrumentUId, decimal>> ProvidePrices(
        IReadOnlyCollection<InstrumentUId> instrumentUIds,
        CancellationToken token)
    {
        var response = await client.MarketData.GetLastPricesAsync(new GetLastPricesRequest
        {
            InstrumentId = { instrumentUIds.Select(i => i.ToString()) }
        }, cancellationToken: token);

        return response.LastPrices.ToDictionary(i => InstrumentUId.From(i.InstrumentUid), i => Convert(i.Price));
    }

    private static decimal Convert(Quotation quotation) => new DecimalProto(quotation.Units, quotation.Nano);
}