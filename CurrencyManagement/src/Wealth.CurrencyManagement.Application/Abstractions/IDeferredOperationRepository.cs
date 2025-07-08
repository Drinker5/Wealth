namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface IDeferredOperationRepository
{
    Task Add(OutboxMessage message, CancellationToken cancellationToken = default);
    Task<OutboxMessage?> LoadAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Guid>> LoadUnprocessed(int take, CancellationToken cancellationToken = default);
    void Remove(OutboxMessage outboxMessage);
}
