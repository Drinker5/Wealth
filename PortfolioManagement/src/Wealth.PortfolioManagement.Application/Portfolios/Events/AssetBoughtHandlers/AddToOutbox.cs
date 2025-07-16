using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Application.Events;
using Wealth.BuildingBlocks.Application.Services;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events.AssetBoughtHandlers;

public class AddToOutbox(IOutboxRepository outboxRepository, IInstrumentService instrumentService) : IDomainEventHandler<AssetBought>
{
    public async Task Handle(AssetBought notification, CancellationToken cancellationToken)
    {
        var instrumentInfo = await instrumentService.GetInstrumentInfo(notification.InstrumentId);
        if (instrumentInfo == null)
            throw new ApplicationException("The instrument could not be found");

        switch (instrumentInfo.Type)
        {
            case InstrumentType.Stock:
                await outboxRepository.Add(new StockBoughtIntegrationEvent
                {
                    PortfolioId = notification.PortfolioId,
                    InstrumentId = notification.InstrumentId,
                    TotalPrice = notification.TotalPrice,
                    Quantity = notification.Quantity,
                }, cancellationToken);
                break;
            case InstrumentType.Bond:
                await outboxRepository.Add(new BondBoughtIntegrationEvent
                {
                    PortfolioId = notification.PortfolioId,
                    InstrumentId = notification.InstrumentId,
                    TotalPrice = notification.TotalPrice,
                    Quantity = notification.Quantity,
                }, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}