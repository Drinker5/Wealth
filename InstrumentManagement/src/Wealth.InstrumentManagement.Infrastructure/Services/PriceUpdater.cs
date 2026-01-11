using Microsoft.Extensions.Options;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Options;
using Wealth.InstrumentManagement.Application.Providers;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.Infrastructure.Services;

public sealed class PriceUpdater(
    IOptions<PriceUpdaterOptions> priceUpdaterOptions,
    IPricesRepository pricesRepository,
    IInstrumentPricesProvider pricesProvider) : IPriceUpdater
{
    public async Task UpdatePrices(CancellationToken token)
    {
        var instrumentUIds = await pricesRepository.GetOld(priceUpdaterOptions.Value.OlderThan, token);
        var prices = await pricesProvider.ProvidePrices(instrumentUIds, token);

        var instrumentUIdPrices = prices.Select(i => new InstrumentUIdPrice(i.Key, i.Value)).ToArray();
        await pricesRepository.UpdatePrices(instrumentUIdPrices, token);
    }
}