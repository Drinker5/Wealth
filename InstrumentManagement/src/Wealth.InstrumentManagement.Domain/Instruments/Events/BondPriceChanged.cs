using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record BondPriceChanged : DomainEvent
{
    public BondId BondId { get; set; }
    public ISIN ISIN { get; set; }
    public Money NewPrice { get; set; }
}