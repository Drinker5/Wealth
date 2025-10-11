using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.PortfolioManagement.Application.Portfolios.Commands;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events.CurrencyDepositedHandlers;

public class CreateOperation(ICqrsInvoker mediator) : IDomainEventHandler<CurrencyDeposited>
{
    public Task Handle(CurrencyDeposited notification, CancellationToken cancellationToken)
    {
        return mediator.Command(
            new AddOperation(new CurrencyOperation
            {
                Id = OperationId.NewId(),
                Date = Clock.Now,
                Type = CurrencyOperationType.Deposit,
                Amount = notification.Money,
                PortfolioId = notification.PortfolioId,
            }),
            cancellationToken);
    }
}