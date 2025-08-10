using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies;

public record StrategyComponentId(Guid Id) : IIdentity
{
    public static StrategyComponentId New() => new(Guid.NewGuid());

    public override string ToString()
    {
        return Id.ToString();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static implicit operator Guid(StrategyComponentId walletId)
    {
        return walletId.Id;
    }

    public static implicit operator StrategyComponentId(Guid id)
    {
        return new StrategyComponentId(id);
    }
}