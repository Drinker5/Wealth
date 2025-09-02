using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StockStrategyComponentWeightChanged(StrategyId StrategyId, StockId StockId, float Weight) : DomainEvent;

public record BondStrategyComponentWeightChanged(StrategyId StrategyId, BondId BondId, float Weight) : DomainEvent;

public record CurrencyStrategyComponentWeightChanged(StrategyId StrategyId, CurrencyId CurrencyId, float Weight) : DomainEvent;