using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Application.CommandScheduler;
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
        return Task.CompletedTask; // TODO integration event
    }
}
