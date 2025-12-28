using Eventso.Subscription;
using Wealth.Aggregation.Application.Commands;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Events;

public sealed class OperationEventHandler(ICqrsInvoker mediator) : IMessageHandler<IReadOnlyCollection<Operation>>
{
    public async Task Handle(IReadOnlyCollection<Operation> messages, CancellationToken token)
    {
        await mediator.Command(new InsertOperations(messages), token);
    }
}