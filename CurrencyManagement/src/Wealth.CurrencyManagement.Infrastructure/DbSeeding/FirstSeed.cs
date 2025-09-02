using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

namespace Wealth.CurrencyManagement.Infrastructure.DbSeeding;

public class FirstSeed : IDbSeeder<WealthDbContext>
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (!context.Currencies.Any())
            context.Currencies.AddRange(GetPredefinedCurrencies());

        if (!context.ExchangeRates.Any())
            context.ExchangeRates.AddRange(GetPredefinedExchangeRates());

        await context.SaveChangesAsync();
    }

    private static IEnumerable<Currency> GetPredefinedCurrencies()
    {
        yield return Currency.Create(CurrencyCode.RUB, "Ruble", "₽");
        yield return Currency.Create(CurrencyCode.USD, "Dollar", "$");
    }

    private static IEnumerable<ExchangeRate> GetPredefinedExchangeRates()
    {
        yield return ExchangeRate.Create(CurrencyCode.RUB, CurrencyCode.USD, 2.42m, new DateOnly(2020, 10, 10));
        yield return ExchangeRate.Create(CurrencyCode.RUB, CurrencyCode.USD, 3.42m, new DateOnly(2020, 10, 11));
        yield return ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.RUB, 10.10m, new DateOnly(2020, 10, 10));
        yield return ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.RUB, 12.34m, new DateOnly(2020, 10, 11));
    }
}