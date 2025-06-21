using Microsoft.EntityFrameworkCore;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Utilities;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

namespace Wealth.CurrencyManagement.Infrastructure.Repositories;

internal class OutboxRepository : IOutboxRepository
{
    private readonly WealthDbContext context;

    public OutboxRepository(WealthDbContext context)
    {
        this.context = context;
    }

    public async Task Add(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        await context.OutboxMessages.AddAsync(message, cancellationToken);
    }

    public async Task<OutboxMessage?> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context
            .OutboxMessages
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Guid>> LoadUnprocessed(int take, CancellationToken cancellationToken = default)
    {
        return await context.OutboxMessages.AsNoTracking()
            .OrderBy(i => i.OccurredOn) // old first
            .Where(i => i.ProcessedDate == null && (i.ProcessingDate < Clock.Now || i.ProcessingDate == null))
            .Select(x => x.Id)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public void Remove(OutboxMessage outboxMessage)
    {
        context.OutboxMessages.Remove(outboxMessage);
    }
}