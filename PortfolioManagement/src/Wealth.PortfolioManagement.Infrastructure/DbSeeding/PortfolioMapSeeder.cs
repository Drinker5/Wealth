using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Providers;
using Wealth.PortfolioManagement.Infrastructure.Providers;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.DbSeeding;

public class PortfolioMapSeeder(IOptions<PortfolioMapOptions> options) : IDbSeeder
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (options.Value.PortfolioIdMap.Count == 0)
            return;

        await context.PortfolioIdMaps.AddRangeAsync(options.Value.PortfolioIdMap.Select(i => new PortfolioIdMap
        {
            PortfolioId = new PortfolioId(i.Value),
            AccountId = i.Key
        }));
    }
}