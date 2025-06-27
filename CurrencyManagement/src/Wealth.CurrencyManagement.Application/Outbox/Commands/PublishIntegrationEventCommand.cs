using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Abstractions;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

internal record PublishIntegrationEventCommand(IntegrationEvent IntegrationEvent) : ICommand;