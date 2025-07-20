using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Domain.Repositories;

namespace Wealth.DepositManagement.Application.Deposits.Commands;

public class DepositWithdrawHandler(IDepositRepository repository) : ICommandHandler<DepositWithdraw>
{
    public Task Handle(DepositWithdraw request, CancellationToken cancellationToken)
    {
        return repository.Withdraw(request.DepositId, request.Withdrawal);
    }
}