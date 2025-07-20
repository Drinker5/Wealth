using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Domain.Repositories;

namespace Wealth.DepositManagement.Application.Deposits.Commands;

public class CreateDepositOperationHandler(IDepositOperationRepository repository) : ICommandHandler<CreateDepositOperation>
{
    public Task Handle(CreateDepositOperation request, CancellationToken cancellationToken)
    {
        return repository.CreateOperation(request.DepositId, request.Type, request.Date, request.Money);
    }
}