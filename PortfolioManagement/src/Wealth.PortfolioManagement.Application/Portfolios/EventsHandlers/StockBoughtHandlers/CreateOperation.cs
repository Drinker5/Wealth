using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.EventsHandlers.StockBoughtHandlers;

public class AssetBoughtEventHandler(IOperationRepository operationRepository) : IDomainEventHandler<StockBought>
{
    public Task Handle(StockBought notification, CancellationToken cancellationToken)
    {
        return operationRepository.CreateOperation(new StockTradeOperation
        {
            Quantity = notification.Quantity,
            Date = Clock.Now,
            StockId = notification.StockId,
            Money = notification.TotalPrice,
            PortfolioId = notification.PortfolioId,
            Type = TradeOperationType.Buy,
        });
    }
}