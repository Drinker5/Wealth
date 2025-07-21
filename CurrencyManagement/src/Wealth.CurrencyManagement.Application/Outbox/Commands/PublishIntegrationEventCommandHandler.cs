using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

internal class PublishIntegrationEventCommandHandler(IOutboxRepository outboxRepository) : ICommandHandler<PublishIntegrationEventCommand>
{
    public async Task Handle(PublishIntegrationEventCommand request, CancellationToken cancellationToken)
    {
        await outboxRepository.Add(request.IntegrationEvent, cancellationToken);
    }
}