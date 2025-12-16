using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Domain.Repositories;

public interface IStrategyRepository
{
    public Task<StrategyId> CreateStrategy(string name, CancellationToken token = default);
    public Task<IReadOnlyList<Strategy>> GetStrategies(CancellationToken token = default);
    public Task<Strategy?> GetStrategy(StrategyId strategyId, CancellationToken token = default);
    public Task RenameStrategy(StrategyId strategyId, string newName, CancellationToken token = default);
    public Task AddStrategyComponent(StrategyId strategyId, StockId stockId, decimal weight, CancellationToken token = default);
    public Task AddStrategyComponent(StrategyId strategyId, BondId bondId, decimal weight, CancellationToken token = default);
    public Task AddStrategyComponent(StrategyId strategyId, CurrencyId currencyId, decimal weight, CancellationToken token = default);
    public Task RemoveStrategyComponent(StrategyId strategyId, StockId instrumentId, CancellationToken token = default);
    public Task RemoveStrategyComponent(StrategyId strategyId, BondId instrumentId, CancellationToken token = default);
    public Task RemoveStrategyComponent(StrategyId strategyId, CurrencyId instrumentId, CancellationToken token = default);
    public Task ChangeStrategyComponentWeight(StrategyId strategyId, StockId instrumentId, decimal weight, CancellationToken token = default);
    public Task ChangeStrategyComponentWeight(StrategyId strategyId, BondId instrumentId, decimal weight, CancellationToken token = default);
    public Task ChangeStrategyComponentWeight(StrategyId strategyId, CurrencyId instrumentId, decimal weight, CancellationToken token = default);
    public Task ChangeMasterStrategy(StrategyId strategyId, MasterStrategy masterStrategy, CancellationToken token = default);
}