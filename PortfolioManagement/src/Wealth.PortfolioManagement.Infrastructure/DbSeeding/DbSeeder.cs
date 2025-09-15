using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.DbSeeding;

public class DbSeeder(IEnumerable<IDbSeeder> seeders) : IDbSeeder<WealthDbContext>
{
    public async Task SeedAsync(WealthDbContext context)
    {
        foreach (var seeder in seeders)
            await seeder.SeedAsync(context);
        
        await context.SaveChangesAsync();
    }
}