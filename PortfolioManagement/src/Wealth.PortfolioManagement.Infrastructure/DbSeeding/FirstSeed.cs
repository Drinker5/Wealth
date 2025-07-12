using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;
using Wealth.PortfolioManagement.Domain;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.ValueObjects;
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
        var foo = Portfolio.Create(1, "Foo");
        foo.Buy(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000001")), new Money("RUB", 13.4m), 3);
        foo.Buy(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000002")), new Money("USD", 43.5m), 23);
        foo.Deposit("USD", 23.23m);
        foo.Deposit("RUB", 3.23m);
        yield return foo;
        
        var bar = Portfolio.Create(2, "Bar");
        bar.Buy(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000003")), new Money("RUB", 23.6m), 32);
        bar.Buy(new InstrumentId(new Guid("00000000-0000-0000-0000-000000000004")), new Money("USD", 33.7m), 12);
        bar.Deposit("USD", 13.23m);
        bar.Deposit("RUB", 123.3m);
        yield return bar;
    }
}