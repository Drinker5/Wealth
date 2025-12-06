using System.Text.Json.Serialization;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Domain.Strategies;

[JsonDerivedType(typeof(StockStrategyComponent), typeDiscriminator: (byte)InstrumentType.Stock)]
[JsonDerivedType(typeof(BondStrategyComponent), typeDiscriminator: (byte)InstrumentType.Bond)]
[JsonDerivedType(typeof(CurrencyAssetStrategyComponent), typeDiscriminator: (byte)InstrumentType.CurrencyAsset)]
[JsonDerivedType(typeof(CurrencyStrategyComponent), typeDiscriminator: (byte)InstrumentType.Currency)]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
public abstract class StrategyComponent : IEntity
{
    public float Weight { get; set; }
    public int Id { get; init; }
}

public class StockStrategyComponent : StrategyComponent
{
    private StockId? _stockId;

    public StockId StockId
    {
        get => _stockId ??= new StockId(Id);
        init => Id = value;
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
        get => _bondId ??= new BondId(Id);
        init => Id = value;
    }

    public override int GetHashCode()
    {
        return BondId.GetHashCode();
    }
}

public class CurrencyAssetStrategyComponent : StrategyComponent
{
    private CurrencyId? _currencyId;

    public CurrencyId CurrencyId
    {
        get => _currencyId ??= new CurrencyId(Id);
        init => Id = value;
    }

    public override int GetHashCode()
    {
        return CurrencyId.GetHashCode();
    }
}

public class CurrencyStrategyComponent : StrategyComponent
{
    private CurrencyCode? _currency;

    public CurrencyCode Currency
    {
        get => _currency ??= (CurrencyCode)Id;
        init => Id = (int)value;
    }

    public override int GetHashCode()
    {
        return Currency.GetHashCode();
    }
}