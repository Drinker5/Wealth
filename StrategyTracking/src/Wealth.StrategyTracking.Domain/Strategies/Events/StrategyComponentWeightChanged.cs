using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StrategyComponentWeightChanged(StrategyId StrategyId, StrategyComponentId ComponentId, float Weight) : DomainEvent;