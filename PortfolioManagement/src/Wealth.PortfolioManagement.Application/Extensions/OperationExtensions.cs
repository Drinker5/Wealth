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
            case CurrencyOperation currencyOperation:
                operationProto.CurrencyOperation = currencyOperation.ToProto();
                break;
            case StockTradeOperation stockTradeOperation:
                operationProto.StockTrade = stockTradeOperation.ToProto();
                break;
            case BondBrokerFeeOperation bondBrokerFeeOperation:
                operationProto.BondBrokerFee = bondBrokerFeeOperation.ToProto();
                break;
            case BondCouponOperation bondCouponOperation:
                operationProto.BondCoupon = bondCouponOperation.ToProto();
                break;
            case BondTradeOperation bondTradeOperation:
                operationProto.BondTrade = bondTradeOperation.ToProto();
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

    private static CurrencyOperationProto ToProto(this CurrencyOperation operation) => new()
    {
        PortfolioId = operation.PortfolioId,
        Amount = operation.Amount,
        Type = operation.Type switch
        {
            CurrencyOperationType.Deposit => CurrencyOperationTypeProto.Deposit,
            CurrencyOperationType.Withdraw => CurrencyOperationTypeProto.Withdraw,
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
}