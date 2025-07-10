using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.Decorators;

internal class UnitOfWorkCreateOutboxMessagesDecorator : IUnitOfWork
{
    private readonly IUnitOfWork decorated;
    private readonly IDeferredOperationRepository repository;
    private readonly IJsonSerializer jsonSerializer;
    private readonly WealthDbContext context;

    public UnitOfWorkCreateOutboxMessagesDecorator(
        IUnitOfWork decorated,
        IDeferredOperationRepository repository,
        IJsonSerializer jsonSerializer,
        WealthDbContext context)
    {
        this.decorated = decorated;
        this.repository = repository;
        this.jsonSerializer = jsonSerializer;
        this.context = context;
    }

    public Task<IDisposable> BeginTransaction() => decorated.BeginTransaction();

    public async Task<int> Commit(CancellationToken cancellationToken = default)
    {
        await CreateOutboxMessages(cancellationToken);
        return await decorated.Commit(cancellationToken);
    }

    private async Task CreateOutboxMessages(CancellationToken cancellationToken = default)
    {
        var aggregate = GetModifiedAggregate();
        if (aggregate is null)
            return;

        foreach (var domainEvent in aggregate.DomainEvents)
        {
            var outboxMessage = OutboxMessage.Create(
                jsonSerializer,
                Clock.Now,
                domainEvent);
            await repository.Add(outboxMessage, cancellationToken);
        }
        
        aggregate.ClearDomainEvents();
    }

    private AggregateRoot? GetModifiedAggregate()
    {
        var modifiedAggregates = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(aggregate => aggregate.Entity.DomainEvents != null && aggregate.Entity.DomainEvents.Any())
            .Select(aggregate => aggregate.Entity);

        if (modifiedAggregates.Count() > 1)
            throw new Exception("Updating multiple aggregates in a transaction is not allowed");

        return modifiedAggregates.FirstOrDefault();
    }
}