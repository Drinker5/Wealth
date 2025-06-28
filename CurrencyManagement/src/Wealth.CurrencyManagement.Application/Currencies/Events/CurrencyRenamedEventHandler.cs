using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.DomainEvents;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Currencies.Events;

internal class CurrencyRenamedEventHandler : IDomainEventHandler<CurrencyRenamed>
{
    private readonly ICommandsScheduler scheduler;

    public CurrencyRenamedEventHandler(ICommandsScheduler scheduler)
    {
        this.scheduler = scheduler;
    }
    
    public Task Handle(CurrencyRenamed notification, CancellationToken cancellationToken)
    {
        return scheduler.EnqueuePublishingEventAsync(new SomeIntegrationEvent());
    }
}
