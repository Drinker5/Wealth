using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.DbSeeding;

public class FirstSeed : IDbSeeder<WealthDbContext>
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (!context.Portfolios.Any())
            await context.Portfolios.AddRangeAsync(GetPredefinedPortfolios());

        await context.SaveChangesAsync();
    }

    private static IEnumerable<Portfolio> GetPredefinedPortfolios()
    {
        var foo = Portfolio.Create("Foo");
        foo.Buy(new StockId(1), new Money("RUB", 13.4m), 3);
        foo.Buy(new StockId(2), new Money("USD", 43.5m), 23);
        foo.Deposit(new Money("USD", 23.23m));
        foo.Deposit(new Money("RUB", 3.23m));
        yield return foo;

        var bar = Portfolio.Create("Bar");
        bar.Buy(new BondId(1), new Money("RUB", 23.6m), 32);
        bar.Buy(new BondId(2), new Money("USD", 33.7m), 12);
        bar.Deposit(new Money("USD", 13.23m));
        bar.Deposit(new Money("RUB", 123.3m));
        yield return bar;
    }
}