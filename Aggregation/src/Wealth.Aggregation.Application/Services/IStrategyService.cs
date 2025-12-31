using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Services;

public interface IStrategyService
{
    Task<Strategy?> GetStrategy(StrategyId strategyId, CancellationToken token);
}

public record Strategy(
    StrategyId Id,
    string Name,
    IReadOnlyCollection<StrategyComponent> Components);

public record struct StrategyComponent(InstrumentIdType IdType, decimal Weight);
