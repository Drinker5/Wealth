using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

namespace Wealth.CurrencyManagement.Infrastructure.Repositories;

internal class DeferredOperationRepository : IDeferredOperationRepository
{
    private readonly WealthDbContext context;

    public DeferredOperationRepository(WealthDbContext context)
    {
        this.context = context;
    }

    public async Task Add(DefferedCommand message, CancellationToken cancellationToken = default)
    {
        await context.DefferedCommands.AddAsync(message, cancellationToken);
    }

    public async Task<DefferedCommand?> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context
            .DefferedCommands
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Guid>> LoadUnprocessed(int take, CancellationToken cancellationToken = default)
    {
        return await context.DefferedCommands.AsNoTracking()
            .OrderBy(i => i.OccurredOn) // old first
            .Where(i => i.ProcessedDate == null && (i.ProcessingDate < Clock.Now || i.ProcessingDate == null))
            .Select(x => x.Id)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public void Remove(DefferedCommand outboxMessage)
    {
        context.DefferedCommands.Remove(outboxMessage);
    }
}