using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies;

public sealed class CurrencyAssetStrategyComponent : StrategyComponent
{
    private CurrencyId? _currencyId;

    public CurrencyId CurrencyId
    {
        get => _currencyId ??= new CurrencyId(Id);
        init => Id = value;
    }

    public override int GetHashCode() => CurrencyId.GetHashCode();
}