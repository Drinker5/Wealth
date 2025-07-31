using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies;

public readonly record struct StrategyId(int Id) : IIdentity
{
    public static StrategyId New() => new StrategyId(0);

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Id.ToString();
    }
    
    public static implicit operator int(StrategyId id)
    {
        return id.Id;
    }
    
    public static implicit operator StrategyId(int id)
    {
        return new StrategyId(id);
    }
}