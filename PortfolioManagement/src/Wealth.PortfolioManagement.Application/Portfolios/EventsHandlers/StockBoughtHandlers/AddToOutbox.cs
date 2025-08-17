using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Application.Services;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.EventsHandlers.StockBoughtHandlers;

public class AddToOutbox(IOutboxRepository outboxRepository, IInstrumentService instrumentService) : IDomainEventHandler<StockBought>
{
    public async Task Handle(StockBought notification, CancellationToken cancellationToken)
    {
        var instrumentInfo = await instrumentService.GetStockInfo(notification.StockId);
        if (instrumentInfo == null)
            throw new ApplicationException("The instrument could not be found");

        await outboxRepository.Add(
            notification,
            new StockBoughtIntegrationEvent
            {
                PortfolioId = notification.PortfolioId,
                InstrumentId = notification.StockId,
                TotalPrice = notification.TotalPrice,
                Quantity = notification.Quantity,
            }, notification.PortfolioId.ToString(), cancellationToken);
    }
}