using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies;

public class StrategyComponent : IEntity
{
    public InstrumentId InstrumentId { get; init; }
    public float Weight { get; set; }

    public override int GetHashCode()
    {
        return InstrumentId.GetHashCode();
    }
}