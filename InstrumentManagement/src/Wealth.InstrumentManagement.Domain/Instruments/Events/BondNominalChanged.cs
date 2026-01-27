using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record BondNominalChanged : DomainEvent
{
    public BondId BondId { get; set; }
    public Money NewNominal { get; set; }
}