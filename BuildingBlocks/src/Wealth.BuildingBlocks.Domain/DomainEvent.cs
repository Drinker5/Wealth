using MediatR;
using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.BuildingBlocks.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTimeOffset OccurredOn { get; init; } = Clock.Now;

    public override string ToString() => Id.ToString("N");
}