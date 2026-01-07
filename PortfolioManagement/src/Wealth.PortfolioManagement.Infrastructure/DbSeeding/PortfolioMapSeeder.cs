using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wealth.PortfolioManagement.Application.Options;
using Wealth.PortfolioManagement.Infrastructure.Providers;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.DbSeeding;

public class PortfolioMapSeeder(IOptions<PortfolioMapOptions> options) : IDbSeeder
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (options.Value.PortfolioIdMap.Count == 0)
            return;

        foreach (var portfolioIdMap in options.Value.PortfolioIdMap)
        {
            await context.Database.ExecuteSqlRawAsync(
                """
                INSERT INTO "PortfolioIdMaps" ("AccountId", "PortfolioId")
                VALUES ({0}, {1})
                ON CONFLICT ("AccountId") 
                DO UPDATE SET "PortfolioId" = EXCLUDED."PortfolioId"
                """,
                [portfolioIdMap.Key, portfolioIdMap.Value]);
        }
    }
}