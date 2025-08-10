using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events.AssetBoughtHandlers;

public class AssetBoughtEventHandler(IOperationRepository operationRepository) : IDomainEventHandler<AssetBought>
{
    public Task Handle(AssetBought notification, CancellationToken cancellationToken)
    {
        return operationRepository.CreateOperation(new TradeOperation
        {
            Quantity = notification.Quantity,
            Date = Clock.Now,
            InstrumentId = notification.InstrumentId,
            Money = notification.TotalPrice,
            PortfolioId = notification.PortfolioId,
            Type = TradeOperationType.Buy,
        });
    }
}