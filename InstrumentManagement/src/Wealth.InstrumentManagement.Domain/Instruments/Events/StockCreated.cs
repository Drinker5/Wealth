using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public class StockCreated : IDomainEvent
{
    public InstrumentId Id { get; set; }
    public string Name { get; set; }
    public ISIN ISIN { get; set; }
}