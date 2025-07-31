using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies.Events;

public record StrategyComponentWeightChanged(StrategyId StrategyId, InstrumentId InstrumentId, float Weight) : DomainEvent;