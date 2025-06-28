using MediatR;
using Wealth.BuildingBlocks.Domain;
using Wealth.CurrencyManagement.Domain.Abstractions;

namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : DomainEvent
{
}