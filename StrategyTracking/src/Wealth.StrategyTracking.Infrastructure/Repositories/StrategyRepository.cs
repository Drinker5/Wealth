using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Repositories;
using Wealth.StrategyTracking.Domain.Strategies;
using Wealth.StrategyTracking.Infrastructure.UnitOfWorks;

namespace Wealth.StrategyTracking.Infrastructure.Repositories;

public class StrategyRepository(WealthDbContext context) : IStrategyRepository
{
    public async Task<StrategyId> CreateStrategy(string name)
    {
        var strategy = Strategy.Create(name);
        await context.Strategies.AddAsync(strategy);
        return strategy.Id;
    }

    public async Task<IReadOnlyList<Strategy>> GetStrategies()
    {
        return await context.Strategies.AsNoTracking()
            .Include(i => i.Components)
            .ToListAsync();
    }

    public Task<Strategy?> GetStrategy(StrategyId id)
    {
        return context.Strategies
            .Include(i => i.Components)
            .SingleOrDefaultAsync(i => i.Id == id);
    }

    public async Task RenameStrategy(StrategyId strategyId, string newName)
    {
        var strategy = await GetStrategy(strategyId);

        strategy?.Rename(newName);
    }

    public async Task AddStrategyComponent(StrategyId strategyId, StockId stockId, float weight)
    {
        var strategy = await GetStrategy(strategyId);
        strategy?.AddOrUpdateComponent(stockId, weight);
    }

    public async Task AddStrategyComponent(StrategyId strategyId, BondId bondId, float weight)
    {
        var strategy = await GetStrategy(strategyId);
        strategy?.AddOrUpdateComponent(bondId, weight);
    }

    public async Task AddStrategyComponent(StrategyId strategyId, CurrencyId currencyId, float weight)
    {
        var strategy = await GetStrategy(strategyId);
        strategy?.AddOrUpdateComponent(currencyId, weight);
    }

    public async Task RemoveStrategyComponent(StrategyId strategyId, StockId instrumentId)
    {
        var strategy = await GetStrategy(strategyId);
        strategy?.RemoveStrategyComponent(instrumentId);
    }

    public async Task RemoveStrategyComponent(StrategyId strategyId, BondId instrumentId)
    {
        var strategy = await GetStrategy(strategyId);
        strategy?.RemoveStrategyComponent(instrumentId);
    }

    public async Task RemoveStrategyComponent(StrategyId strategyId, CurrencyId instrumentId)
    {
        var strategy = await GetStrategy(strategyId);
        strategy?.RemoveStrategyComponent(instrumentId);
    }

    public async Task ChangeStrategyComponentWeight(StrategyId strategyId, StockId instrumentId, float weight)
    {
        var strategy = await GetStrategy(strategyId);
        strategy?.AddOrUpdateComponent(instrumentId, weight);
    }
    
    public async Task ChangeStrategyComponentWeight(StrategyId strategyId, BondId instrumentId, float weight)
    {
        var strategy = await GetStrategy(strategyId);
        strategy?.AddOrUpdateComponent(instrumentId, weight);
    }
    
    public async Task ChangeStrategyComponentWeight(StrategyId strategyId, CurrencyId instrumentId, float weight)
    {
        var strategy = await GetStrategy(strategyId);
        strategy?.AddOrUpdateComponent(instrumentId, weight);
    }
}