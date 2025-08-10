using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StrategyComponentRemoved(StrategyId StrategyId, StrategyComponentId ComponentId) : DomainEvent;