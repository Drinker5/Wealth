using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.Decorators;

internal class UnitOfWorkCreateOutboxMessagesDecorator : IUnitOfWork
{
    private readonly IUnitOfWork decorated;
    private readonly IOutboxRepository repository;
    private readonly IJsonSerializer jsonSerializer;
    private readonly WealthDbContext context;

    public UnitOfWorkCreateOutboxMessagesDecorator(
        IUnitOfWork decorated,
        IOutboxRepository repository,
        IJsonSerializer jsonSerializer,
        WealthDbContext context)
    {
        this.decorated = decorated;
        this.repository = repository;
        this.jsonSerializer = jsonSerializer;
        this.context = context;
    }

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

        var domainEvents = aggregate.DomainEvents;
        foreach (var domainEvent in domainEvents)
        {
            var outboxMessage = OutboxMessage.Create(
                jsonSerializer,
                Clock.Now,
                domainEvent);
            await repository.Add(outboxMessage, cancellationToken);
        }
    }

    private AggregateRoot? GetModifiedAggregate()
    {
        var modifiedAggregates = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(aggregate => aggregate.Entity.DomainEvents.Any())
            .Select(aggregate => aggregate.Entity);

        if (modifiedAggregates.Count() > 1)
            throw new Exception("Updating multiple aggregates in a transaction is not allowed");

        return modifiedAggregates.FirstOrDefault();
    }
}