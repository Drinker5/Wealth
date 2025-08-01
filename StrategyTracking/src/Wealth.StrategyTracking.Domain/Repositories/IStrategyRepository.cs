using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Domain.Repositories;

public interface IStrategyRepository
{
    public Task<StrategyId> CreateStrategy(string name);
    public Task<IReadOnlyList<Strategy>> GetStrategies();
    public Task<Strategy?> GetStrategy(StrategyId strategyId);
    public Task RenameStrategy(StrategyId strategyId, string newName);
    public Task AddStrategyComponent(StrategyId strategyId, InstrumentId instrumentId, float weight);
    public Task RemoveStrategyComponent(StrategyId strategyId, InstrumentId instrumentId);
    public Task ChangeStrategyComponentWeight(StrategyId strategyId, InstrumentId instrumentId, float weight);
}