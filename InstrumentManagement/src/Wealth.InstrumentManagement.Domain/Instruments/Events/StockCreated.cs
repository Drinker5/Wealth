using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record StockCreated() : DomainEvent
{
    public StockId StockId { get; init; }
    public string Index { get; init; }
    public string Name { get; init; }
    public ISIN Isin { get; init; }
    public FIGI Figi { get; init; }
}