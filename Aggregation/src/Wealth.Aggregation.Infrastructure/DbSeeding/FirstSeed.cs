using Wealth.Aggregation.Domain;
using Wealth.Aggregation.Infrastructure.UnitOfWorks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;

namespace Wealth.Aggregation.Infrastructure.DbSeeding;

public class FirstSeed : IDbSeeder<WealthDbContext>
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (!context.StockAggregations.Any())
            context.StockAggregations.AddRange(GetStockAggregations());

        await context.SaveChangesAsync();
    }

    private static IEnumerable<StockAggregation> GetStockAggregations()
    {
        var foo = new StockAggregation(
            new InstrumentId(new Guid("00000000-0000-0000-0000-000000000001")),
            "First",
            new Money("RUB", 120.35m),
            new Money("RUB", 10m),
            1);

        foo.Buy(5, new Money("RUB", 500m));
        foo.AddDividend(new Money("RUB", 23.23m));
        yield return foo;
    }
}