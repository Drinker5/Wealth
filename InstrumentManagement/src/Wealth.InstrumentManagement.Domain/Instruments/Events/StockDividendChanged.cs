using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public class StockDividendChanged : IDomainEvent
{
    public InstrumentId Id { get; set; }
    public ISIN ISIN { get; set; }
    public Dividend NewDividend { get; set; }
}