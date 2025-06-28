using MediatR;
using Wealth.BuildingBlocks.Domain;
using Wealth.CurrencyManagement.Domain.Utilities;

namespace Wealth.CurrencyManagement.Domain.Abstractions;

public record DomainEvent : IDomainEvent, INotification
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredAt { get; } = Clock.Now;
}