using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StrategyComponentRemoved(StrategyId StrategyId, StrategyComponent Component) : DomainEvent;