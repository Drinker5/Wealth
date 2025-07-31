using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StrategyRenamed(StrategyId StrategyId, string NewName) : DomainEvent;