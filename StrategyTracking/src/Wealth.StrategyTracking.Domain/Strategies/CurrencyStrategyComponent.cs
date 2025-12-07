using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies;

public sealed class CurrencyStrategyComponent : StrategyComponent
{
    private CurrencyCode? _currency;

    public CurrencyCode Currency
    {
        get => _currency ??= (CurrencyCode)Id;
        init => Id = (int)value;
    }

    public override int GetHashCode() => Currency.GetHashCode();
}