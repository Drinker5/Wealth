using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Commands;

public sealed record InsertOperations(IReadOnlyCollection<Operation> Operations) : ICommand;

public sealed class InsertOperationsHandler(IOperationsRepository repository) : ICommandHandler<InsertOperations>
{
    public Task Handle(InsertOperations command, CancellationToken token)
    {
        return repository.Upsert(command.Operations, token);
    }
}