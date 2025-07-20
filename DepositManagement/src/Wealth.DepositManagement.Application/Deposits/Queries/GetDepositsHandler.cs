using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Domain.Repositories;

namespace Wealth.DepositManagement.Application.Deposits.Queries;

public class GetDepositsHandler(IDepositRepository repository) : IQueryHandler<GetDeposits, IEnumerable<Deposit>>
{
    public Task<IEnumerable<Deposit>> Handle(GetDeposits request, CancellationToken cancellationToken)
    {
        return repository.GetDeposits();
    }
}