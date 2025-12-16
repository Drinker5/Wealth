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
        foo.AddOrUpdateComponent(stockId: 1, weight: 50);
        foo.AddOrUpdateComponent(bondId: 1, weight: 30);
        foo.AddOrUpdateComponent(currencyId: 1, weight: 12);
        foo.AddOrUpdateComponent(currency: CurrencyCode.Cny, weight: 8);
        yield return foo;

        var bar = Strategy.Create("Seed-strategy-2");
        bar.AddOrUpdateComponent(stockId: 2, weight: 50);
        bar.AddOrUpdateComponent(bondId: 2, weight: 50);
        yield return bar;
    }
}