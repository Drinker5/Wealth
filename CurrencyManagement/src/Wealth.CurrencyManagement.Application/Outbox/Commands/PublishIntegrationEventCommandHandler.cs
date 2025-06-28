using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

internal class PublishIntegrationEventCommandHandler(IEventBus eventBus) : ICommandHandler<PublishIntegrationEventCommand>
{
    public async Task Handle(PublishIntegrationEventCommand request, CancellationToken cancellationToken)
    {
        await eventBus.Publish(request.IntegrationEvent, cancellationToken);
    }
}