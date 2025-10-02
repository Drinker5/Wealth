using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record StockCreated : DomainEvent
{
    public StockId StockId { get; set; }
    public string Name { get; set; }
    public ISIN Isin { get; set; }
    public FIGI Figi { get; set; }
}