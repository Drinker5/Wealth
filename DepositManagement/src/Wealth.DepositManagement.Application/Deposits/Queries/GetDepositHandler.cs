using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Domain.Repositories;

namespace Wealth.DepositManagement.Application.Deposits.Queries;

public class GetDepositHandler(IDepositRepository repository) : IQueryHandler<GetDeposit, Deposit?>
{
    public Task<Deposit?> Handle(GetDeposit request, CancellationToken cancellationToken)
    {
        return repository.GetDeposit(request.DepositId);
    }
}