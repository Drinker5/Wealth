using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StrategyComponentAdded(StrategyId StrategyId, StrategyComponent Component) : DomainEvent;