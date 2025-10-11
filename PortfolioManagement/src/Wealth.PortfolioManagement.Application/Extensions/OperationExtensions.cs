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
            case BondCouponOperation bondCouponOperation:
            case BondTradeOperation bondTradeOperation:
            case StockBrokerFeeOperation stockBrokerFeeOperation:
            case StockDelistOperation stockDelistOperation:
            case StockDividendOperation stockDividendOperation:
            case StockDividendTaxOperation stockDividendTaxOperation:
            case StockSplitOperation stockSplitOperation:
            case TradeOperation tradeOperation:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }

        return operationProto;
    }

    private static BondAmortizationOperationProto ToProto(this BondAmortizationOperation operation)
    {
        return new BondAmortizationOperationProto
        {
            PortfolioId = operation.PortfolioId,
            BondId = operation.BondId,
            Amount = operation.Amount
        };
    }
    
    private static CurrencyOperationProto ToProto(this CurrencyOperation operation)
    {
        return new CurrencyOperationProto
        {
            PortfolioId = operation.PortfolioId,
            Amount = operation.Amount,
            Type = operation.Type switch
            {
                CurrencyOperationType.Deposit => CurrencyOperationProto.Types.CurrencyOperationTypeProto.Deposit,
                CurrencyOperationType.Withdraw => CurrencyOperationProto.Types.CurrencyOperationTypeProto.Withdraw,
                _ => throw new ArgumentOutOfRangeException(nameof(operation))
            }
        };
    }
    
    private static StockTradeOperationProto ToProto(this StockTradeOperation operation)
    {
        return new StockTradeOperationProto
        {
            PortfolioId = operation.PortfolioId,
            StockId = operation.StockId,
            Amount = operation.Amount,
            Quantity = operation.Quantity,
            Type = operation.Type switch
            {
                TradeOperationType.Buy => StockTradeOperationProto.Types.TradeOperationTypeProto.Buy,
                TradeOperationType.Sell => StockTradeOperationProto.Types.TradeOperationTypeProto.Sell,
                _ => throw new ArgumentOutOfRangeException(nameof(operation))
            }
        };
    }
}