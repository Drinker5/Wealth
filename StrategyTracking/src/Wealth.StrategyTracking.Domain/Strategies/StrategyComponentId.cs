using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies;

public record StrategyComponentId(int Id) : IIdentity
{
    public static StrategyComponentId New() => new(0);

    public override string ToString()
    {
        return Id.ToString();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    public static implicit operator int(StrategyComponentId value)
    {
        return value.Id;
    }
    
    public static implicit operator StrategyComponentId(int value)
    {
        return new StrategyComponentId(value);
    }
}