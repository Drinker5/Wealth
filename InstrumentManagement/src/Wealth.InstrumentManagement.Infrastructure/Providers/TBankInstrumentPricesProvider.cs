using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Providers;
using Wealth.InstrumentManagement.Application.Repositories;
using InstrumentType = Wealth.BuildingBlocks.Domain.Common.InstrumentType;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public sealed class TBankInstrumentPricesProvider(
    IOptions<TBankInstrumentsProviderOptions> options,
    IStocksRepository stocksRepository,
    IBondsRepository bondsRepository,
    ICurrenciesRepository currenciesRepository) : IInstrumentPricesProvider
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);

    public async Task<IReadOnlyDictionary<InstrumentUId, decimal>> ProvidePrices(
        IReadOnlyCollection<InstrumentUIdType> instrumentUIds,
        CancellationToken token)
    {
        var response = await client.MarketData.GetLastPricesAsync(new GetLastPricesRequest
        {
            InstrumentId = { instrumentUIds.Select(i => i.UId.Value.ToString()) }
        }, cancellationToken: token);
        var rawPrices = response.LastPrices.ToDictionary(i => InstrumentUId.From(i.InstrumentUid), i => Convert(i.Price));

        var stocks = await stocksRepository.GetStocks(instrumentUIds.Where(i => i.Type == InstrumentType.Stock).Select(i => i.UId), token);
        var bonds = await bondsRepository.GetBonds(instrumentUIds.Where(i => i.Type == InstrumentType.Bond).Select(i => i.UId), token);
        var currencies = await currenciesRepository.GetCurrencies(instrumentUIds.Where(i => i.Type == InstrumentType.Currency).Select(i => i.UId), token);

        var prices = new Dictionary<InstrumentUId, decimal>();
        foreach (var stock in stocks)
        {
            // Stocks: price * lot
            prices[stock.Key] = rawPrices[stock.Key] * stock.Value.LotSize;
        }

        foreach (var bond in bonds)
        {
            // TODO: Bonds: price / 100 * nominal
            prices[bond.Key] = rawPrices[bond.Key];
        }

        foreach (var currency in currencies)
        {
            // TODO: Currencies: price * lot / nominal
            prices[currency.Key] = rawPrices[currency.Key];
        }

        return prices;
    }

    private static decimal Convert(Quotation? quotation)
        => quotation == null ? 0 : new DecimalProto(quotation.Units, quotation.Nano);
}