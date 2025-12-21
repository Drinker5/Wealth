using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record CurrencyCreated(
    CurrencyId CurrencyId,
    string Name,
    FIGI Figi,
    InstrumentId InstrumentId) : DomainEvent;