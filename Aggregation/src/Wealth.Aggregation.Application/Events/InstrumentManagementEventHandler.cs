using Eventso.Subscription;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Models;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;

namespace Wealth.Aggregation.Application.Events;

public sealed class InstrumentManagementEventHandler(ICqrsInvoker cqrsInvoker)
    : IMessageHandler<IReadOnlyCollection<InstrumentPriceChangedIntegrationEvent>>
{
    public Task Handle(IReadOnlyCollection<InstrumentPriceChangedIntegrationEvent> messages, CancellationToken token = default)
    {
        var prices = messages
            .Select(i => new InstrumentPrice(new InstrumentIdType(i.InstrumentId, i.InstrumentType.FromProto()), i.NewPrice))
            .ToList();

        if (prices.Count == 0)
            return Task.CompletedTask;

        return cqrsInvoker.Command(new ChangePrice(prices), token);
    }
}