using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record  StockDividendChanged : DomainEvent
{
    public InstrumentId InstrumentId { get; set; }
    public ISIN ISIN { get; set; }
    public Dividend NewDividend { get; set; }
}