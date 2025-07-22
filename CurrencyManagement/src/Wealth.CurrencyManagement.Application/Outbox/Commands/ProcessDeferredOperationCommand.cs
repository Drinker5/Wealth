using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.Outbox.Commands;

public record ProcessDeferredOperationCommand : ICommand
{
    public ProcessDeferredOperationCommand(Guid operationId)
    {
        OperationId = operationId;
    }

    public Guid OperationId { get; }
}
