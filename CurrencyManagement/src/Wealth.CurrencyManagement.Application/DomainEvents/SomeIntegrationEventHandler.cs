using Microsoft.Extensions.Logging;
using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.DomainEvents;

public record SomeIntegrationEventHandler : IIntegrationEventHandler<SomeIntegrationEvent>
{
    private readonly ILogger<SomeIntegrationEventHandler> logger;

    public SomeIntegrationEventHandler(ILogger<SomeIntegrationEventHandler> logger)
    {
        this.logger = logger;
    }
    
    public Task Handle(SomeIntegrationEvent @event, CancellationToken token = default)
    {
        logger.LogInformation("Handling integration event: {Name}", @event.GetType().Name);
        throw new Exception("Something went wrong");
    }
}