using System.Text.Json.Serialization;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies;

[JsonDerivedType(typeof(StockStrategyComponent), typeDiscriminator: (byte)StrategyComponentType.Stock)]
[JsonDerivedType(typeof(BondStrategyComponent), typeDiscriminator: (byte)StrategyComponentType.Bond)]
[JsonDerivedType(typeof(CurrencyStrategyComponent), typeDiscriminator: (byte)StrategyComponentType.Currency)]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
public abstract class StrategyComponent : IEntity
{
    public float Weight { get; set; }
    public string Id { get; protected init; }
}

public enum StrategyComponentType : byte
{
    Stock,
    Bond,
    Currency
}

public class StockStrategyComponent : StrategyComponent
{
    private readonly StockId _stockId;

    public StockId StockId
    {
        get => _stockId;
        init
        {
            Id = value.ToString();
            _stockId = value;
        }
    }

    public override int GetHashCode()
    {
        return StockId.GetHashCode();
    }
}

public class BondStrategyComponent : StrategyComponent
{
    private BondId? _bondId;

    public BondId BondId
    {
        get => _bondId ??= new BondId(int.Parse(Id));
        init => Id = value.ToString();
    }

    public override int GetHashCode()
    {
        return BondId.GetHashCode();
    }
}

public class CurrencyStrategyComponent : StrategyComponent
{
    private CurrencyId? _currencyId;

    public CurrencyId CurrencyId
    {
        get => _currencyId ??= new CurrencyId(Id);
        init => Id = value.ToString();
    }

    public override int GetHashCode()
    {
        return CurrencyId.GetHashCode();
    }
}