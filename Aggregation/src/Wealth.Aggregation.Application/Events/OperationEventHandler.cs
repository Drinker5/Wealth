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
            case OperationProto.VariantOneofCase.BondAmortizationOperation:
            case OperationProto.VariantOneofCase.CurrencyOperation:
            case OperationProto.VariantOneofCase.StockTrade:
                await mediator.Command(new StockTrade(
                        message.Id,
                        message.StockTrade.PortfolioId,
                        message.StockTrade.StockId,
                        message.StockTrade.Quantity,
                        message.StockTrade.Amount,
                        message.StockTrade.Type),
                    token);
                return;
            case OperationProto.VariantOneofCase.BondBrokerFee:
            case OperationProto.VariantOneofCase.BondCoupon:
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