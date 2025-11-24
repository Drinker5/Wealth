using MediatR;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Infrastructure.DbSeeding.Seeds;

public class FirstSeed(
    IUnitOfWork unitOfWork,
    IBondsRepository bondsRepository,
    IStocksRepository stocksRepository,
    ICurrenciesRepository currenciesRepository) : IDbSeeder
{
    public async Task SeedAsync(CancellationToken token = default)
    {
        await unitOfWork.Transaction(Action, token);
    }

    private async Task<Unit> Action(CancellationToken token)
    {
        var bonds = await bondsRepository.GetBonds();
        if (!bonds.Any())
        {
            await CreateBonds(token);
            await CreateStocks(token);
            await CreateCurrencies(token);
        }

        return Unit.Value;
    }

    private async Task CreateBonds(CancellationToken token)
    {
        var bond1 = await bondsRepository.CreateBond("test-bond-1", new ISIN("000000000001"), new FIGI("000000000001"), token);
        await bondsRepository.ChangePrice(bond1, new Money(CurrencyCode.Rub, 12.34m));
        await bondsRepository.ChangeCoupon(bond1, new Coupon(CurrencyCode.Rub, 33m));

        var bond2 = await bondsRepository.CreateBond("test-bond-2", new ISIN("000000000002"), new FIGI("000000000002"), token);
        await bondsRepository.ChangePrice(bond2, new Money(CurrencyCode.Usd, 2.12m));
        await bondsRepository.ChangeCoupon(bond2, new Coupon(CurrencyCode.Rub, 44m));
    }

    private async Task CreateStocks(CancellationToken token)
    {
        var stock1 = await stocksRepository.CreateStock("test-stock-1", new ISIN("000000000003"), new FIGI("000000000003"), 10, token);
        await stocksRepository.ChangePrice(stock1, new Money(CurrencyCode.Rub, 111m));
        await stocksRepository.ChangeDividend(stock1, new Dividend(CurrencyCode.Usd, 222m));

        var stock2 = await stocksRepository.CreateStock("test-stock-2", new ISIN("000000000004"), new FIGI("000000000004"), LotSize.One, token);
        await stocksRepository.ChangePrice(stock2, new Money(CurrencyCode.Usd, 222m));
        await stocksRepository.ChangeDividend(stock2, new Dividend(CurrencyCode.Usd, 333m));
    }
    
    private async Task CreateCurrencies(CancellationToken token)
    {
        var currency1 = await currenciesRepository.CreateCurrency("test-currency-1", new FIGI("000000000006"), token);
        await currenciesRepository.ChangePrice(currency1, new Money(CurrencyCode.Rub, 123m));

        var currency2 = await currenciesRepository.CreateCurrency("test-currency-2", new FIGI("000000000007"), token);
        await currenciesRepository.ChangePrice(currency2, new Money(CurrencyCode.Usd, 234m));
    }
}