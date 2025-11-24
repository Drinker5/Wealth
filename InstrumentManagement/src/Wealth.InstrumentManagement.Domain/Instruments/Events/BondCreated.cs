using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record BondCreated : DomainEvent
{
    public BondId BondId { get; set; }
    public string Name { get; set; }
    public ISIN Isin { get; set; }
    public FIGI Figi { get; set; }
}