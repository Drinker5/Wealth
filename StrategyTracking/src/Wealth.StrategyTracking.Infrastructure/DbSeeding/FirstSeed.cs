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
        var foo = Strategy.Create("Test-strategy-1");
        foo.AddOrUpdateComponent(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000001")), 50f);
        foo.AddOrUpdateComponent(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000002")), 50f);
        yield return foo;
        
        var bar = Strategy.Create("Test-strategy-2");
        bar.AddOrUpdateComponent(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000002")), 30f);
        bar.AddOrUpdateComponent(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000003")), 30f);
        bar.AddOrUpdateComponent(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000004")), 40f);
        yield return bar;
    }
}