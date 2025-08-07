using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies;

public abstract class StrategyComponent : IEntity
{
    public float Weight { get; set; }
    public abstract StrategyComponentType Type { get; }
}

public enum StrategyComponentType
{
    Stock,
    Bond,
    Currency
}

public class StockStrategyComponent : StrategyComponent, IEquatable<StockStrategyComponent>
{
    public required StockId StockId { get; init; }
    public override StrategyComponentType Type => StrategyComponentType.Stock;

    public override int GetHashCode()
    {
        return StockId.GetHashCode();
    }

    public bool Equals(StockStrategyComponent? other)
    {
        if (other is null) return false;
        return StockId.Equals(other.StockId);
    }
}

public class BondStrategyComponent : StrategyComponent
{
    public required BondId BondId { get; init; }
    public override StrategyComponentType Type => StrategyComponentType.Bond;

    public override int GetHashCode()
    {
        return BondId.GetHashCode();
    }
}

public class CurrencyStrategyComponent : StrategyComponent
{
    public required CurrencyId CurrencyId { get; init; }
    public override StrategyComponentType Type => StrategyComponentType.Currency;

    public override int GetHashCode()
    {
        return CurrencyId.GetHashCode();
    }
}