using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events.CurrencyDepositedHandlers;

public class CreateOperation(IOperationRepository operationRepository) : IDomainEventHandler<CurrencyDeposited>
{
    public Task Handle(CurrencyDeposited notification, CancellationToken cancellationToken)
    {
        return operationRepository.CreateOperation(new CurrencyOperation
        {
            Date = Clock.Now,
            Type = CurrencyOperationType.Deposit,
            Money = new Money(notification.CurrencyId, notification.Amount),
            PortfolioId = notification.PortfolioId,
        });
    }
}