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
    public int Id { get; init; }
    public decimal Weight { get; set; }
}