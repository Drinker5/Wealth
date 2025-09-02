using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StockStrategyComponentRemoved(StrategyId StrategyId, StockId StockId) : DomainEvent;

public record BondStrategyComponentRemoved(StrategyId StrategyId, BondId BondId) : DomainEvent;

public record CurrencyStrategyComponentRemoved(StrategyId StrategyId, CurrencyId CurrencyId) : DomainEvent;