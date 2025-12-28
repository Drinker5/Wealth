using Eventso.Subscription;
using Wealth.Aggregation.Application.Commands;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Infrastructure.EventBus;

public sealed class OperationProtoDeserializer : IMessageDeserializer
{
    public ConsumedMessage Deserialize<TContext>(ReadOnlySpan<byte> message, in TContext context) where TContext : IDeserializationContext
    {
        if (message.Length == 0)
            return ConsumedMessage.Skipped;

        var proto = OperationProto.Parser.ParseFrom(message);
        return new ConsumedMessage(Map(proto));
    }

    private static Operation Map(OperationProto message)
    {
        switch (message.VariantCase)
        {
            case OperationProto.VariantOneofCase.StockTrade:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.StockTrade.PortfolioId,
                    message.StockTrade.StockId.Id,
                    InstrumentType.Stock,
                    message.StockTrade.Amount,
                    Map(message.StockTrade.Type),
                    message.StockTrade.Quantity);
            case OperationProto.VariantOneofCase.CurrencyTrade:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.CurrencyTrade.PortfolioId,
                    message.CurrencyTrade.CurrencyId.Id,
                    InstrumentType.CurrencyAsset,
                    message.CurrencyTrade.Amount,
                    Map(message.CurrencyTrade.Type),
                    message.CurrencyTrade.Quantity);
            case OperationProto.VariantOneofCase.BondCoupon:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.BondCoupon.PortfolioId,
                    message.BondCoupon.BondId.Id,
                    InstrumentType.Bond,
                    message.BondCoupon.Amount,
                    OperationType.Coupon);
            case OperationProto.VariantOneofCase.StockDividend:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.StockDividend.PortfolioId,
                    message.StockDividend.StockId.Id,
                    InstrumentType.Stock,
                    message.StockDividend.Amount,
                    OperationType.Dividend);
            case OperationProto.VariantOneofCase.StockDividendTax:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.StockDividendTax.PortfolioId,
                    message.StockDividendTax.StockId.Id,
                    InstrumentType.Stock,
                    message.StockDividendTax.Amount,
                    OperationType.DividendTax);
            case OperationProto.VariantOneofCase.StockBrokerFee:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.StockBrokerFee.PortfolioId,
                    message.StockBrokerFee.StockId.Id,
                    InstrumentType.Stock,
                    message.StockBrokerFee.Amount,
                    OperationType.BrokerFee);
            case OperationProto.VariantOneofCase.MoneyOperation:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.MoneyOperation.PortfolioId,
                    InstrumentId: -1,
                    InstrumentType.Currency,
                    message.MoneyOperation.Amount,
                    Map(message.MoneyOperation.Type));
            case OperationProto.VariantOneofCase.BondBrokerFee:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.BondBrokerFee.PortfolioId,
                    message.BondBrokerFee.BondId.Id,
                    InstrumentType.Bond,
                    message.BondBrokerFee.Amount,
                    OperationType.BrokerFee);
            case OperationProto.VariantOneofCase.CurrencyBrokerFee:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.CurrencyBrokerFee.PortfolioId,
                    message.CurrencyBrokerFee.CurrencyId.Id,
                    InstrumentType.CurrencyAsset,
                    message.CurrencyBrokerFee.Amount,
                    OperationType.BrokerFee);
            case OperationProto.VariantOneofCase.BondAmortizationOperation:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.BondAmortizationOperation.PortfolioId,
                    message.BondAmortizationOperation.BondId.Id,
                    InstrumentType.Bond,
                    message.BondAmortizationOperation.Amount,
                    OperationType.Amortization);
            case OperationProto.VariantOneofCase.BondTrade:
                return new Operation(
                    message.Id,
                    message.Date.ToDateTime(),
                    message.BondTrade.PortfolioId,
                    message.BondTrade.BondId.Id,
                    InstrumentType.Bond,
                    message.BondTrade.Amount,
                    Map(message.BondTrade.Type),
                    message.BondTrade.Quantity);
            case OperationProto.VariantOneofCase.StockDelist:
            case OperationProto.VariantOneofCase.StockSplit:
            case OperationProto.VariantOneofCase.None:
            default:
                throw new ArgumentOutOfRangeException($"Unknown variant case {message.VariantCase}");
        }
    }

    private static OperationType Map(TradeOperationTypeProto proto) =>
        proto switch
        {
            TradeOperationTypeProto.Buy => OperationType.Buy,
            TradeOperationTypeProto.Sell => OperationType.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(proto), proto, null)
        };

    private static OperationType Map(MoneyOperationTypeProto proto) =>
        proto switch
        {
            MoneyOperationTypeProto.Deposit => OperationType.Deposit,
            MoneyOperationTypeProto.Withdraw => OperationType.Withdraw,
            _ => throw new ArgumentOutOfRangeException(nameof(proto), proto, null)
        };
}