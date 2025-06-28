using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public class StockCreated : IDomainEvent
{
    public InstrumentId Id { get; set; }
    public string Name { get; set; }
    public ISIN ISIN { get; set; }
    public Dividend Dividend { get; set; }
}