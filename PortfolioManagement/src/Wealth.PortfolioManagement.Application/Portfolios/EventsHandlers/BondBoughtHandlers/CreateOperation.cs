using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.EventsHandlers.BondBoughtHandlers;

public class AssetBoughtEventHandler(IOperationRepository operationRepository) : IDomainEventHandler<BondBought>
{
    public Task Handle(BondBought notification, CancellationToken cancellationToken)
    {
        return operationRepository.CreateOperation(new BondTradeOperation
        {
            Id = OperationId.NewId(),
            Quantity = notification.Quantity,
            Date = Clock.Now,
            BondId = notification.BondId,
            Amount = notification.TotalPrice,
            PortfolioId = notification.PortfolioId,
            Type = TradeOperationType.Buy,
        }, CancellationToken.None);
    }
}