using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

public record ProcessOutboxCommand : ICommand
{
    public ProcessOutboxCommand(Guid outboxMessageId)
    {
        MessageId = outboxMessageId;
    }

    public Guid MessageId { get; }
}
