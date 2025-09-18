using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Providers;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public class PortfolioIdProvider(IDbContextFactory<WealthDbContext> contextFactory) : IPortfolioIdProvider
{
    private readonly ConcurrentDictionary<string, PortfolioId> cache = new();

    public async ValueTask<PortfolioId> GetPortfolioIdByAccountId(string accountId, CancellationToken token = default)
    {
        if (cache.TryGetValue(accountId, out var cachedId))
            return cachedId;

        var context = await contextFactory.CreateDbContextAsync(token);
        var portfolioIdMap = await context.PortfolioIdMaps
            .SingleAsync(i => i.AccountId == accountId, cancellationToken: token);
        cache.AddOrUpdate(accountId, portfolioIdMap.PortfolioId, (k, v) => v);
        return portfolioIdMap.PortfolioId;
    }
}