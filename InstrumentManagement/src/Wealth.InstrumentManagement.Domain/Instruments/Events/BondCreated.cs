using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record BondCreated : DomainEvent
{
    public required BondId BondId { get; init; }
    public required string Name { get; init; }
    public required ISIN Isin { get; init; }
    public required FIGI Figi { get; init; }
    public required InstrumentId InstrumentId { get; init; }
}