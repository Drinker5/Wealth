using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Domain.Repositories;

namespace Wealth.DepositManagement.Application.Deposits.Commands;

public class DepositInvestHandler(IDepositRepository repository)  : ICommandHandler<DepositInvest>
{
    public Task Handle(DepositInvest request, CancellationToken cancellationToken)
    {
        return repository.Invest(request.DepositId, request.Investment);
    }
}