using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies;

public record StrategyComponentId(Guid Value) : IIdentity
{
    public static StrategyComponentId New() => new(Guid.NewGuid());

    public override string ToString()
    {
        return Value.ToString();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static implicit operator Guid(StrategyComponentId walletId)
    {
        return walletId.Value;
    }

    public static implicit operator StrategyComponentId(Guid id)
    {
        return new StrategyComponentId(id);
    }
}