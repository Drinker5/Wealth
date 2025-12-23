using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record BondNameChanged(BondId BondId, string Name) : DomainEvent;