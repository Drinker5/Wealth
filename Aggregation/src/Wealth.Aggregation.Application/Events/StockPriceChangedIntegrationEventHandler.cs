using Eventso.Subscription;
using Wealth.Aggregation.Application.Commands;
using Wealth.BuildingBlocks.Application;


namespace Wealth.Aggregation.Application.Events;

public sealed class StockPriceChangedIntegrationEventHandler(ICqrsInvoker cqrsInvoker)
    : IMessageHandler<IReadOnlyCollection<StockPriceChangedIntegrationEvent>>
{
    public async Task Handle(IReadOnlyCollection<StockPriceChangedIntegrationEvent> messages, CancellationToken token = default)
    {
        foreach (var message in messages)
            await Handle(token, message);
    }

    private Task Handle(CancellationToken token, StockPriceChangedIntegrationEvent message)
        => cqrsInvoker.Command(
            new StockChangePrice
            {
                StockId = message.StockId,
                NewPrice = message.NewPrice,
            },
            token);
}