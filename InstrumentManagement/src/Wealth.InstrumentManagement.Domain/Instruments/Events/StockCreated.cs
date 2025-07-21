using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record StockCreated : DomainEvent
{
    public InstrumentId InstrumentId { get; set; }
    public string Name { get; set; }
    public ISIN ISIN { get; set; }
}