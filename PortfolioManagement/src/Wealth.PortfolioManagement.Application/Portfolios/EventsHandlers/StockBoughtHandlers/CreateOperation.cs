using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.PortfolioManagement.Application.Portfolios.Commands;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.EventsHandlers.StockBoughtHandlers;

public class AssetBoughtEventHandler(ICqrsInvoker mediator) : IDomainEventHandler<StockBought>
{
    public Task Handle(StockBought notification, CancellationToken cancellationToken)
    {
        return mediator.Command(
            new AddOperation(new StockTradeOperation
            {
                Id = OperationId.NewId(),
                Quantity = notification.Quantity,
                Date = Clock.Now,
                StockId = notification.StockId,
                Amount = notification.TotalPrice,
                PortfolioId = notification.PortfolioId,
                Type = TradeOperationType.Buy,
            }),
            cancellationToken);
    }
}