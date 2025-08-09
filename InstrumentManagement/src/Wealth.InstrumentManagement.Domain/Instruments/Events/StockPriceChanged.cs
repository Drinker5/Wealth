using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record StockPriceChanged : DomainEvent
{
    public StockId StockId { get; set; }
    public ISIN ISIN { get; set; }
    public Money NewPrice { get; set; }
}