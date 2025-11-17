using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Domain;
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
                await mediator.Command(new BondCoupon(
                        message.Id,
                        message.Date.ToDateTime(),
                        message.BondCoupon.PortfolioId,
                        message.BondCoupon.BondId,
                        message.BondCoupon.Amount),
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
            case OperationProto.VariantOneofCase.BondAmortizationOperation:
            case OperationProto.VariantOneofCase.BondBrokerFee:
            case OperationProto.VariantOneofCase.BondTrade:
            case OperationProto.VariantOneofCase.StockBrokerFee:
            case OperationProto.VariantOneofCase.StockDelist:
            case OperationProto.VariantOneofCase.StockDividend:
            case OperationProto.VariantOneofCase.StockDividendTax:
            case OperationProto.VariantOneofCase.StockSplit:
            case OperationProto.VariantOneofCase.None:
            default:
                throw new ArgumentOutOfRangeException($"Unknown variant case {message.VariantCase}");
        }
    }
}