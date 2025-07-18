using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public class InstrumentPriceChanged : IDomainEvent
{
    public InstrumentId Id { get; set; }
    public ISIN ISIN { get; set; }
    public Money NewPrice { get; set; }
    public InstrumentType Type { get; set; }
}