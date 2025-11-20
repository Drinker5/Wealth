using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.Repositories;

namespace Wealth.DepositManagement.Application.Deposits.Commands;

public class CreateDepositHandler(IDepositRepository repository) : ICommandHandler<CreateDeposit, DepositId>
{
    public Task<DepositId> Handle(CreateDeposit request, CancellationToken cancellationToken)
    {
        return repository.Create(request.Name, request.Yield, request.Currency);
    }
}