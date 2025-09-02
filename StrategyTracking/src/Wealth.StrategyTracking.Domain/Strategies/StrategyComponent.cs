using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies;

public abstract class StrategyComponent : IEntity
{
    public float Weight { get; set; }
}

public enum StrategyComponentType : byte
{
    Stock,
    Bond,
    Currency
}

public class StockStrategyComponent : StrategyComponent
{
    public required StockId StockId { get; init; }

    public override int GetHashCode()
    {
        return StockId.GetHashCode();
    }
}

public class BondStrategyComponent : StrategyComponent
{
    public required BondId BondId { get; init; }

    public override int GetHashCode()
    {
        return BondId.GetHashCode();
    }
}

public class CurrencyStrategyComponent : StrategyComponent
{
    public required CurrencyId CurrencyId { get; init; }

    public override int GetHashCode()
    {
        return CurrencyId.GetHashCode();
    }
}