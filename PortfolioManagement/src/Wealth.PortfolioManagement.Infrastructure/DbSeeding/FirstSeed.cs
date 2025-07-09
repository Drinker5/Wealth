using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.DbSeeding;

public class FirstSeed : IDbSeeder<WealthDbContext>
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (!context.Portfolios.Any())
            context.Portfolios.AddRange(GetPredefinedPortfolios());

        await context.SaveChangesAsync();
    }

    private static IEnumerable<Portfolio> GetPredefinedPortfolios()
    {
        var foo = Portfolio.Create(new Guid("00000000-0000-0000-0000-000000000001"), "Foo");
        foo.AddAsset(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000001")), "000000000001", 3);
        foo.AddAsset(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000002")), "000000000002", 23);
        foo.AddCurrency("USD", 23.23m);
        foo.AddCurrency("RUB", 3.23m);
        yield return foo;
        
        var bar = Portfolio.Create(new Guid("00000000-0000-0000-0000-000000000002"), "Bar");
        bar.AddAsset(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000003")), "000000000003", 32);
        bar.AddAsset(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000004")), "000000000004", 32);
        bar.AddCurrency("USD", 13.23m);
        bar.AddCurrency("RUB", 123.3m);
        yield return bar;
    }
}