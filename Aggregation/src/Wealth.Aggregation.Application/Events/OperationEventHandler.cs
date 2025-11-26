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
                await mediator.Command(new StockTrade(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.StockTrade.PortfolioId,
                        message.StockTrade.StockId,
                        message.StockTrade.Quantity,
                        message.StockTrade.Amount,
                        message.StockTrade.Type),
                    token);
                return;
            case OperationProto.VariantOneofCase.BondCoupon:
                await mediator.Command(new BondCurrencyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.BondCoupon.PortfolioId,
                        message.BondCoupon.BondId,
                        message.BondCoupon.Amount,
                        BondCurrencyOperationType.Coupon),
                    token);
                return;
            case OperationProto.VariantOneofCase.StockDividend:
                await mediator.Command(new StockCurrencyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.StockDividend.PortfolioId,
                        message.StockDividend.StockId,
                        message.StockDividend.Amount,
                        StockCurrencyOperationType.Dividend),
                    token);
                return;
            case OperationProto.VariantOneofCase.StockDividendTax:
                await mediator.Command(new StockCurrencyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.StockDividendTax.PortfolioId,
                        message.StockDividendTax.StockId,
                        message.StockDividendTax.Amount,
                        StockCurrencyOperationType.DividendTax),
                    token);
                return;
            case OperationProto.VariantOneofCase.StockBrokerFee:
                await mediator.Command(new StockCurrencyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.StockBrokerFee.PortfolioId,
                        message.StockBrokerFee.StockId,
                        message.StockBrokerFee.Amount,
                        StockCurrencyOperationType.BrokerFee),
                    token);
                return;
            case OperationProto.VariantOneofCase.CurrencyOperation:
                await mediator.Command(new CurrencyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.CurrencyOperation.PortfolioId,
                        message.CurrencyOperation.Amount,
                        message.CurrencyOperation.Type),
                    token);
                return;
            case OperationProto.VariantOneofCase.BondBrokerFee:
                await mediator.Command(new BondCurrencyOperation(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.BondBrokerFee.PortfolioId,
                        message.BondBrokerFee.BondId,
                        message.BondBrokerFee.Amount,
                        BondCurrencyOperationType.BrokerFee),
                    token);
                return;
            case OperationProto.VariantOneofCase.CurrencyBrokerFee:
            case OperationProto.VariantOneofCase.CurrencyTrade:
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