using Wealth.Aggregation.Application.Commands;
using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Events;

public sealed class OperationEventHandler(ICqrsInvoker mediator) : IMessageHandler<OperationProto>
{
    public async Task Handle(OperationProto message, CancellationToken token)
    {
        switch (message.VariantCase)
        {
            case OperationProto.VariantOneofCase.StockTrade:
                await mediator.Command(new StockTradeOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.StockTrade.PortfolioId,
                        message.StockTrade.StockId,
                        message.StockTrade.Quantity,
                        message.StockTrade.Amount,
                        message.StockTrade.Type),
                    token);
                return;
            case OperationProto.VariantOneofCase.CurrencyTrade:
                await mediator.Command(new CurrencyTradeOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.CurrencyTrade.PortfolioId,
                        message.CurrencyTrade.CurrencyId,
                        message.CurrencyTrade.Quantity,
                        message.CurrencyTrade.Amount,
                        message.CurrencyTrade.Type),
                    token);
                return;
            case OperationProto.VariantOneofCase.BondCoupon:
                await mediator.Command(new BondMoneyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.BondCoupon.PortfolioId,
                        message.BondCoupon.BondId,
                        message.BondCoupon.Amount,
                        BondMoneyOperationType.Coupon),
                    token);
                return;
            case OperationProto.VariantOneofCase.StockDividend:
                await mediator.Command(new StockMoneyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.StockDividend.PortfolioId,
                        message.StockDividend.StockId,
                        message.StockDividend.Amount,
                        StockMoneyOperationType.Dividend),
                    token);
                return;
            case OperationProto.VariantOneofCase.StockDividendTax:
                await mediator.Command(new StockMoneyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.StockDividendTax.PortfolioId,
                        message.StockDividendTax.StockId,
                        message.StockDividendTax.Amount,
                        StockMoneyOperationType.DividendTax),
                    token);
                return;
            case OperationProto.VariantOneofCase.StockBrokerFee:
                await mediator.Command(new StockMoneyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.StockBrokerFee.PortfolioId,
                        message.StockBrokerFee.StockId,
                        message.StockBrokerFee.Amount,
                        StockMoneyOperationType.BrokerFee),
                    token);
                return;
            case OperationProto.VariantOneofCase.MoneyOperation:
                await mediator.Command(new MoneyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.MoneyOperation.PortfolioId,
                        message.MoneyOperation.Amount,
                        message.MoneyOperation.Type),
                    token);
                return;
            case OperationProto.VariantOneofCase.BondBrokerFee:
                await mediator.Command(new BondMoneyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.BondBrokerFee.PortfolioId,
                        message.BondBrokerFee.BondId,
                        message.BondBrokerFee.Amount,
                        BondMoneyOperationType.BrokerFee),
                    token);
                return;
            case OperationProto.VariantOneofCase.CurrencyBrokerFee:
                await mediator.Command(new CurrencyMoneyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.CurrencyBrokerFee.PortfolioId,
                        message.CurrencyBrokerFee.CurrencyId,
                        message.CurrencyBrokerFee.Amount,
                        CurrencyMoneyOperationType.BrokerFee),
                    token);
                return;
            case OperationProto.VariantOneofCase.BondAmortizationOperation:
            case OperationProto.VariantOneofCase.BondTrade:
            case OperationProto.VariantOneofCase.StockDelist:
            case OperationProto.VariantOneofCase.StockSplit:
            case OperationProto.VariantOneofCase.None:
            default:
                throw new ArgumentOutOfRangeException($"Unknown variant case {message.VariantCase}");
        }
    }
}