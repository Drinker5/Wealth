using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StrategyCreated(Strategy Strategy) : DomainEvent;