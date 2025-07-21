using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

internal record PublishIntegrationEventCommand(OutboxMessage IntegrationEvent) : ICommand;