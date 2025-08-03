namespace Wealth.BuildingBlocks.Application.CommandScheduler;

public interface IDeferredOperationRepository
{
    Task Add(DefferedCommand message, CancellationToken cancellationToken = default);
    Task<DefferedCommand?> LoadAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Guid>> LoadUnprocessed(int take, CancellationToken cancellationToken = default);
    void Remove(DefferedCommand outboxMessage);
}
