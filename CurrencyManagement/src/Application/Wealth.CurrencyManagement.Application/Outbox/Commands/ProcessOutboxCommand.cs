using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

public record ProcessOutboxCommand : ICommand
{
    public ProcessOutboxCommand(Guid outboxMessageId)
    {
        MessageId = outboxMessageId;
    }

    public Guid MessageId { get; }
}
