using Google.Protobuf.WellKnownTypes;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Application.Extensions;

public static class OperationExtensions
{
    public static OperationProto ToProto(this Operation operation)
    {
        var operationProto = new OperationProto
        {
            Id = operation.Id.Value,
            Date = Timestamp.FromDateTimeOffset(operation.Date),
        };

        switch (operation)
        {
            case BondAmortizationOperation bondAmortizationOperation:
                operationProto.BondAmortizationOperation = bondAmortizationOperation.ToProto();
                break;
            case MoneyOperation moneyOperation:
                operationProto.MoneyOperation = moneyOperation.ToProto();
                break;
            case StockTradeOperation stockTradeOperation:
                operationProto.StockTrade = stockTradeOperation.ToProto();
                break;
            case BondTradeOperation bondTradeOperation:
                operationProto.BondTrade = bondTradeOperation.ToProto();
                break;
            case BondBrokerFeeOperation bondBrokerFeeOperation:
                operationProto.BondBrokerFee = bondBrokerFeeOperation.ToProto();
                break;
            case BondCouponOperation bondCouponOperation:
                operationProto.BondCoupon = bondCouponOperation.ToProto();
                break;
            case StockBrokerFeeOperation stockBrokerFeeOperation:
                operationProto.StockBrokerFee = stockBrokerFeeOperation.ToProto();
                break;
            case StockDelistOperation stockDelistOperation:
                operationProto.StockDelist = stockDelistOperation.ToProto();
                break;
            case StockDividendOperation stockDividendOperation:
                operationProto.StockDividend = stockDividendOperation.ToProto();
                break;
            case StockDividendTaxOperation stockDividendTaxOperation:
                operationProto.StockDividendTax = stockDividendTaxOperation.ToProto();
                break;
            case StockSplitOperation stockSplitOperation:
                operationProto.StockSplit = stockSplitOperation.ToProto();
                break;
            case CurrencyBrokerFeeOperation currencyBrokerFeeOperation:
                operationProto.CurrencyBrokerFee = currencyBrokerFeeOperation.ToProto();
                break;
            case CurrencyTradeOperation currencyTradeOperation:
                operationProto.CurrencyTrade = currencyTradeOperation.ToProto();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }

        return operationProto;
    }

    private static BondAmortizationOperationProto ToProto(this BondAmortizationOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        BondId = operation.BondId,
        Amount = operation.Amount
    };

    private static MoneyOperationProto ToProto(this MoneyOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        Amount = operation.Amount,
        Type = operation.Type switch
        {
            MoneyOperationType.Deposit => MoneyOperationTypeProto.Deposit,
            MoneyOperationType.Withdraw => MoneyOperationTypeProto.Withdraw,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        }
    };

    private static StockTradeOperationProto ToProto(this StockTradeOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        StockId = operation.StockId,
        Amount = operation.Amount,
        Quantity = operation.Quantity,
        Type = operation.Type switch
        {
            TradeOperationType.Buy => TradeOperationTypeProto.Buy,
            TradeOperationType.Sell => TradeOperationTypeProto.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        }
    };

    private static BondBrokerFeeOperationProto ToProto(this BondBrokerFeeOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        BondId = operation.BondId,
        Amount = operation.Amount
    };

    private static BondCouponOperationProto ToProto(this BondCouponOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        BondId = operation.BondId,
        Amount = operation.Amount
    };

    private static BondTradeOperationProto ToProto(this BondTradeOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        BondId = operation.BondId,
        Amount = operation.Amount,
        Quantity = operation.Quantity,
        Type = operation.Type switch
        {
            TradeOperationType.Buy => TradeOperationTypeProto.Buy,
            TradeOperationType.Sell => TradeOperationTypeProto.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        }
    };

    private static StockBrokerFeeOperationProto ToProto(this StockBrokerFeeOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        StockId = operation.StockId,
        Amount = operation.Amount
    };

    private static StockDelistOperationProto ToProto(this StockDelistOperation operation) => new()
    {
        StockId = operation.StockId,
    };

    private static StockDividendOperationProto ToProto(this StockDividendOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        StockId = operation.StockId,
        Amount = operation.Amount
    };

    private static StockDividendTaxOperationProto ToProto(this StockDividendTaxOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        StockId = operation.StockId,
        Amount = operation.Amount
    };

    private static StockSplitOperationProto ToProto(this StockSplitOperation operation) => new()
    {
        StockId = operation.StockId,
        RationOld = operation.Ratio.Old,
        RationNew = operation.Ratio.New,
    };

    private static CurrencyBrokerFeeOperationProto ToProto(this CurrencyBrokerFeeOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        CurrencyId = operation.CurrencyId,
        Amount = operation.Amount
    };

    private static CurrencyTradeOperationProto ToProto(this CurrencyTradeOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        CurrencyId = operation.CurrencyId,
        Amount = operation.Amount,
        Quantity = operation.Quantity,
        Type = operation.Type switch
        {
            TradeOperationType.Buy => TradeOperationTypeProto.Buy,
            TradeOperationType.Sell => TradeOperationTypeProto.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        }
    };
}