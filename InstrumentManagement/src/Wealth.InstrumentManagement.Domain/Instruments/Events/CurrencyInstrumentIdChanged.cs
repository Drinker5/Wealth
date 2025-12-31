using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record CurrencyInstrumentIdChanged(CurrencyId CurrencyId, InstrumentUId InstrumentUId) : DomainEvent;