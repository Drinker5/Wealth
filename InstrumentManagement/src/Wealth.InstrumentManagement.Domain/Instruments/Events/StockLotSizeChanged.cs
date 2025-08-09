using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record StockLotSizeChanged : DomainEvent
{
    public StockId StockId { get; set; }
    public ISIN ISIN { get; set; }
    public int NewLotSize { get; set; }
}