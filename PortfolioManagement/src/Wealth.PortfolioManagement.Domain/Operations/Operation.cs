using System.Text.Json.Serialization;

namespace Wealth.PortfolioManagement.Domain.Operations;

[JsonDerivedType(typeof(BondBrokerFeeOperation), typeDiscriminator: nameof(BondBrokerFeeOperation))]
[JsonDerivedType(typeof(CashOperation), typeDiscriminator: nameof(CashOperation))]
[JsonDerivedType(typeof(CurrencyOperation), typeDiscriminator: nameof(CurrencyOperation))]
[JsonDerivedType(typeof(SplitOperation), typeDiscriminator: nameof(SplitOperation))]
[JsonDerivedType(typeof(StockBrokerFeeOperation), typeDiscriminator: nameof(StockBrokerFeeOperation))]
[JsonDerivedType(typeof(StockDelistOperation), typeDiscriminator: nameof(StockDelistOperation))]
[JsonDerivedType(typeof(BondTradeOperation), typeDiscriminator: nameof(BondTradeOperation))]
[JsonDerivedType(typeof(StockTradeOperation), typeDiscriminator: nameof(StockTradeOperation))]
public abstract class Operation
{
    public string Id { get; init; }
    public DateTimeOffset Date { get; init; }
}