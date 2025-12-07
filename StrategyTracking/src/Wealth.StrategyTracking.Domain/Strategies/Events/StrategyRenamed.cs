using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StrategyRenamed(StrategyId StrategyId, string NewName) : DomainEvent;