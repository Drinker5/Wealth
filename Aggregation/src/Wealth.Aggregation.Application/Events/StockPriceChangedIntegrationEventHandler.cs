using Eventso.Subscription;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Models;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;


namespace Wealth.Aggregation.Application.Events;

public sealed class StockPriceChangedIntegrationEventHandler(ICqrsInvoker cqrsInvoker)
    : IMessageHandler<IReadOnlyCollection<InstrumentPriceChangedIntegrationEvent>>
{
    public Task Handle(IReadOnlyCollection<InstrumentPriceChangedIntegrationEvent> messages, CancellationToken token = default)
    {
        var prices = messages
            .Select(i => new InstrumentPrice(new InstrumentIdType(i.InstrumentId, i.InstrumentType.FromProto()), i.NewPrice))
            .ToList();

        return cqrsInvoker.Command(new ChangePrice(prices), token);
    }
}