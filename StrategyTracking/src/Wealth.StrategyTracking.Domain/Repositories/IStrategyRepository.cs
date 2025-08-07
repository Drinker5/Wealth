using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Domain.Repositories;

public interface IStrategyRepository
{
    public Task<StrategyId> CreateStrategy(string name);
    public Task<IReadOnlyList<Strategy>> GetStrategies();
    public Task<Strategy?> GetStrategy(StrategyId strategyId);
    public Task RenameStrategy(StrategyId strategyId, string newName);
    public Task AddStrategyComponent(StrategyId strategyId, StockId stockId, float weight);
    public Task AddStrategyComponent(StrategyId strategyId, BondId bondId, float weight);
    public Task AddStrategyComponent(StrategyId strategyId, CurrencyId currencyId, float weight);
    public Task RemoveStrategyComponent(StrategyId strategyId, StrategyComponentId instrumentId);
    public Task ChangeStrategyComponentWeight(StrategyId strategyId, StrategyComponentId instrumentId, float weight);
}