using Wealth.BuildingBlocks.Application;

namespace Wealth.StrategyTracking.Application.Strategies.Queries;

public record GetStrategies : IQuery<IEnumerable<StrategyDTO>>;