using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public class StockLotSizeChanged : IDomainEvent
{
    public InstrumentId Id { get; set; }
    public ISIN ISIN { get; set; }
    public int NewLotSize { get; set; }
}