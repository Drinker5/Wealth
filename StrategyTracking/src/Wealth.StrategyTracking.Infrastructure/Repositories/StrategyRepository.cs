using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Repositories;
using Wealth.StrategyTracking.Domain.Strategies;
using Wealth.StrategyTracking.Infrastructure.UnitOfWorks;

namespace Wealth.StrategyTracking.Infrastructure.Repositories;

public class StrategyRepository(WealthDbContext context) : IStrategyRepository
{
    public async Task<StrategyId> CreateStrategy(string name, CancellationToken token = default)
    {
        var strategy = Strategy.Create(name);
        await context.Strategies.AddAsync(strategy, token);
        return strategy.Id;
    }

    public async Task<IReadOnlyList<Strategy>> GetStrategies(CancellationToken token = default)
    {
        return await context.Strategies.AsNoTracking()
            .Include(i => i.Components)
            .ToListAsync(cancellationToken: token);
    }

    public Task<Strategy?> GetStrategy(StrategyId id, CancellationToken token = default)
    {
        return context.Strategies
            .Include(i => i.Components)
            .SingleOrDefaultAsync(i => i.Id == id, cancellationToken: token);
    }

    public async Task RenameStrategy(StrategyId strategyId, string newName, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);

        strategy?.Rename(newName);
    }

    public async Task AddStrategyComponent(StrategyId strategyId, StockId stockId, decimal weight, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.AddOrUpdateComponent(stockId, weight);
    }

    public async Task AddStrategyComponent(StrategyId strategyId, BondId bondId, decimal weight, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.AddOrUpdateComponent(bondId, weight);
    }

    public async Task AddStrategyComponent(StrategyId strategyId, CurrencyId currencyId, decimal weight, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.AddOrUpdateComponent(currencyId, weight);
    }

    public async Task RemoveStrategyComponent(StrategyId strategyId, StockId instrumentId, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.RemoveStrategyComponent(instrumentId);
    }

    public async Task RemoveStrategyComponent(StrategyId strategyId, BondId instrumentId, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.RemoveStrategyComponent(instrumentId);
    }

    public async Task RemoveStrategyComponent(StrategyId strategyId, CurrencyId instrumentId, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.RemoveStrategyComponent(instrumentId);
    }

    public async Task ChangeStrategyComponentWeight(StrategyId strategyId, StockId instrumentId, decimal weight, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.AddOrUpdateComponent(instrumentId, weight);
    }
    
    public async Task ChangeStrategyComponentWeight(StrategyId strategyId, BondId instrumentId, decimal weight, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.AddOrUpdateComponent(instrumentId, weight);
    }
    
    public async Task ChangeStrategyComponentWeight(StrategyId strategyId, CurrencyId instrumentId, decimal weight, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.AddOrUpdateComponent(instrumentId, weight);
    }

    public async Task ChangeMasterStrategy(StrategyId strategyId, MasterStrategy masterStrategy, CancellationToken token = default)
    {
        var strategy = await GetStrategy(strategyId, token);
        strategy?.Follow(masterStrategy);
    }
}