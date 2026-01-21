using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Options;
using Wealth.InstrumentManagement.Application.Providers;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.Infrastructure.Services;

public sealed class PriceUpdater(
    IOptions<PriceUpdaterOptions> priceUpdaterOptions,
    IPricesRepository pricesRepository,
    IInstrumentPricesProvider pricesProvider,
    IStocksRepository stocksRepository,
    IBondsRepository bondsRepository,
    ICurrenciesRepository currenciesRepository,
    ILogger<PriceUpdater> logger) : IPriceUpdater
{
    public async Task UpdatePrices(CancellationToken token)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug("Updating prices");

        var instrumentUIds = await pricesRepository.GetOld(priceUpdaterOptions.Value.OlderThan, token);
        var prices = await pricesProvider.ProvidePrices(instrumentUIds, token);

        var stocks = await stocksRepository.GetStocks(instrumentUIds.Where(i => i.Type == InstrumentType.Stock).Select(i => i.UId), token);
        foreach (var stock in stocks)
            stock.Value.ChangePrice(stock.Value.Price with { Amount = prices[stock.Key] });
        
        var bonds = await bondsRepository.GetBonds(instrumentUIds.Where(i => i.Type == InstrumentType.Bond).Select(i => i.UId), token);
        foreach (var bond in bonds)
            bond.Value.ChangePrice(bond.Value.Price with { Amount = prices[bond.Key] });
            
        var currencies = await currenciesRepository.GetCurrencies(instrumentUIds.Where(i => i.Type == InstrumentType.Currency).Select(i => i.UId), token);
        foreach (var currency in currencies)
            currency.Value.ChangePrice(currency.Value.Price with { Amount = prices[currency.Key] });

        var instrumentUIdPrices = prices.Select(i => new InstrumentUIdPrice(i.Key, i.Value)).ToArray();
        await pricesRepository.UpdatePrices(instrumentUIdPrices, token);
    }
}