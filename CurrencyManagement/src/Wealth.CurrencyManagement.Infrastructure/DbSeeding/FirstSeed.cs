using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;
using Wealth.CurrencyManagement.Domain.ExchangeRates;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

namespace Wealth.CurrencyManagement.Infrastructure.DbSeeding;

public class FirstSeed : IDbSeeder<WealthDbContext>
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (!context.ExchangeRates.Any())
            context.ExchangeRates.AddRange(GetPredefinedExchangeRates());

        await context.SaveChangesAsync();
    }

    private static IEnumerable<ExchangeRate> GetPredefinedExchangeRates()
    {
        yield return ExchangeRate.Create(CurrencyCode.Rub, CurrencyCode.Usd, 2.42m, new DateOnly(2020, 10, 10));
        yield return ExchangeRate.Create(CurrencyCode.Rub, CurrencyCode.Usd, 3.42m, new DateOnly(2020, 10, 11));
        yield return ExchangeRate.Create(CurrencyCode.Usd, CurrencyCode.Rub, 10.10m, new DateOnly(2020, 10, 10));
        yield return ExchangeRate.Create(CurrencyCode.Usd, CurrencyCode.Rub, 12.34m, new DateOnly(2020, 10, 11));
    }
}