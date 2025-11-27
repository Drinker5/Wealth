using System.Text.Json.Serialization;

namespace Wealth.PortfolioManagement.Domain.Operations;

[JsonDerivedType(typeof(BondBrokerFeeOperation), typeDiscriminator: nameof(BondBrokerFeeOperation))]
[JsonDerivedType(typeof(BondCouponOperation), typeDiscriminator: nameof(BondCouponOperation))]
[JsonDerivedType(typeof(BondAmortizationOperation), typeDiscriminator: nameof(BondAmortizationOperation))]
[JsonDerivedType(typeof(MoneyOperation), typeDiscriminator: nameof(MoneyOperation))]
[JsonDerivedType(typeof(StockSplitOperation), typeDiscriminator: nameof(StockSplitOperation))]
[JsonDerivedType(typeof(StockBrokerFeeOperation), typeDiscriminator: nameof(StockBrokerFeeOperation))]
[JsonDerivedType(typeof(StockDividendOperation), typeDiscriminator: nameof(StockDividendOperation))]
[JsonDerivedType(typeof(StockDelistOperation), typeDiscriminator: nameof(StockDelistOperation))]
[JsonDerivedType(typeof(BondTradeOperation), typeDiscriminator: nameof(BondTradeOperation))]
[JsonDerivedType(typeof(StockTradeOperation), typeDiscriminator: nameof(StockTradeOperation))]
[JsonDerivedType(typeof(StockDividendTaxOperation), typeDiscriminator: nameof(StockDividendTaxOperation))]
public abstract record Operation
{
    public required OperationId Id { get; init; }
    public required DateTimeOffset Date { get; init; }
}