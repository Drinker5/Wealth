using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.Queries;

public record GetStrategies : IQuery<IEnumerable<StrategyDTO>>;

public record GetStrategy(StrategyId StrategyId) : IQuery<StrategyDTO?>;