using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.PortfolioManagement.Application.Portfolios.Commands;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.EventsHandlers.BondBoughtHandlers;

public class AssetBoughtEventHandler(ICqrsInvoker mediator) : IDomainEventHandler<BondBought>
{
    public Task Handle(BondBought notification, CancellationToken cancellationToken)
    {
        return mediator.Command(
            new AddOperation(new BondTradeOperation
            {
                Id = OperationId.NewId(),
                Quantity = notification.Quantity,
                Date = Clock.Now,
                BondId = notification.BondId,
                Amount = notification.TotalPrice,
                PortfolioId = notification.PortfolioId,
                Type = TradeOperationType.Buy,
            }),
            cancellationToken);
    }
}