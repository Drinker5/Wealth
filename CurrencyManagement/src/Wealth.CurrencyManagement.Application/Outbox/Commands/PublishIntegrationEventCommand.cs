using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

internal record PublishIntegrationEventCommand(IntegrationEvent IntegrationEvent) : ICommand;