using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.BuildingBlocks.Infrastructure.Repositories;

internal class InMemoryDefferedOperationRepository : IDeferredOperationRepository
{
    private readonly List<DefferedCommand> defferedCommands = [];

    public Task Add(DefferedCommand message, CancellationToken cancellationToken = default)
    {
        defferedCommands.Add(message);
        return Task.CompletedTask;
    }

    public Task<DefferedCommand?> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(defferedCommands.SingleOrDefault(x => x.Id == id));
    }

    public Task<IEnumerable<Guid>> LoadUnprocessed(int take, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Guid>>(defferedCommands
            .OrderBy(i => i.OccurredOn) // old first
            .Where(i => i.ProcessedDate == null && (i.ProcessingDate < Clock.Now || i.ProcessingDate == null))
            .Select(x => x.Id)
            .Take(take)
            .ToList());
    }

    public void Remove(DefferedCommand outboxMessage)
    {
        defferedCommands.Remove(outboxMessage);
    }
}