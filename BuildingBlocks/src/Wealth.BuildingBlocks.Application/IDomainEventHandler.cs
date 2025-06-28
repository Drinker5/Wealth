using MediatR;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.BuildingBlocks.Application;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}