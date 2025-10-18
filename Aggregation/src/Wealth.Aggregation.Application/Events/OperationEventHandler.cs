using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Events;

public sealed class OperationEventHandler : IMessageHandler<OperationProto>
{
    public Task Handle(OperationProto message, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}