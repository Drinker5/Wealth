using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies;

public sealed class StockStrategyComponent : StrategyComponent
{
    private StockId? _stockId;

    public StockId StockId
    {
        get => _stockId ??= new StockId(Id);
        init => Id = value;
    }

    public override int GetHashCode() => StockId.GetHashCode();
}