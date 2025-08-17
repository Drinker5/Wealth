using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Application.Services;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.EventsHandlers.BondBoughtHandlers;

public class AddToOutbox(IOutboxRepository outboxRepository, IInstrumentService instrumentService) : IDomainEventHandler<BondBought>
{
    public async Task Handle(BondBought notification, CancellationToken cancellationToken)
    {
        var instrumentInfo = await instrumentService.GetBondInfo(notification.BondId);
        if (instrumentInfo == null)
            throw new ApplicationException("The instrument could not be found");

        await outboxRepository.Add(
            notification,
            new BondBoughtIntegrationEvent
            {
                PortfolioId = notification.PortfolioId,
                InstrumentId = notification.BondId,
                TotalPrice = notification.TotalPrice,
                Quantity = notification.Quantity,
            }, notification.PortfolioId.ToString(), cancellationToken);
    }
}