using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;
using Wealth.StrategyTracking.Domain.Strategies;
using Wealth.StrategyTracking.Infrastructure.UnitOfWorks;

namespace Wealth.StrategyTracking.Infrastructure.DbSeeding;

public class FirstSeed : IDbSeeder<WealthDbContext>
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (!context.Strategies.Any())
        {
            await context.Strategies.AddRangeAsync(GetPredefinedStrategies());
        }

        await context.SaveChangesAsync();
    }

    private static IEnumerable<Strategy> GetPredefinedStrategies()
    {
        var foo = Strategy.Create("Seed-strategy-1");
        foo.AddOrUpdateComponent(stockId: 1, weight: 50f);
        foo.AddOrUpdateComponent(bondId: 1, weight: 30f);
        foo.AddOrUpdateComponent(currencyId: 1, weight: 12f);
        foo.AddOrUpdateComponent(currency: CurrencyCode.Cny, weight: 8f);
        yield return foo;

        var bar = Strategy.Create("Seed-strategy-2");
        bar.AddOrUpdateComponent(stockId: 2, weight: 50f);
        bar.AddOrUpdateComponent(bondId: 2, weight: 50f);
        yield return bar;
    }
}