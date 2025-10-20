using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Events;

public sealed class OperationEventHandler : IMessageHandler<OperationProto>
{
    public Task Handle(OperationProto message, CancellationToken token)
    {
        switch (message.VariantCase)
        {
            case OperationProto.VariantOneofCase.BondAmortizationOperation:
            case OperationProto.VariantOneofCase.CurrencyOperation:
            case OperationProto.VariantOneofCase.StockTrade:
                throw new NotImplementedException();
            case OperationProto.VariantOneofCase.None:
            default:
                throw new ArgumentOutOfRangeException($"Unknown variant case {message.VariantCase}");
        }
    }
}