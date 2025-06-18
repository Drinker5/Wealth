namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface IOutboxRepository
{
    Task Add(OutboxMessage message, CancellationToken cancellationToken = default);
    Task<OutboxMessage?> LoadAsync(Guid id, CancellationToken cancellationToken = default);
    void Remove(OutboxMessage outboxMessage);
}
