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
        foo.AddOrUpdateComponent(new StockStrategyComponent
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            StockId = 1,
            Weight = 50f,
        });
        foo.AddOrUpdateComponent(new BondStrategyComponent(
        {
            Id = new Guid("00000000-0000-0000-0000-000000000002"),
            BondId = 1,
            Weight = 50f,
        });
        yield return foo;

        var bar = Strategy.Create("Seed-strategy-2");
        bar.AddOrUpdateComponent(new StockStrategyComponent
        {
            Id = new Guid("00000000-0000-0000-0000-000000000003"),
            StockId = 2,
            Weight = 30f,
        });
        bar.AddOrUpdateComponent(new BondStrategyComponent()
        {
            Id =  new Guid("00000000-0000-0000-0000-000000000004"),
            BondId = 2,
            Weight = 30f,
        });
        bar.AddOrUpdateComponent(new CurrencyStrategyComponent()
        {
            Id =  new Guid("00000000-0000-0000-0000-000000000005"),
            CurrencyId = "RUB",
            Weight = 40f,
        });
        yield return bar;
    }
}