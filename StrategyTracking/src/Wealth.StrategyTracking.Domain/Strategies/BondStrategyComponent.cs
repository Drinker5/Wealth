using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies;

public sealed class BondStrategyComponent : StrategyComponent
{
    private BondId? _bondId;

    public BondId BondId
    {
        get => _bondId ??= new BondId(Id);
        init => Id = value;
    }

    public override int GetHashCode() => BondId.GetHashCode();
}