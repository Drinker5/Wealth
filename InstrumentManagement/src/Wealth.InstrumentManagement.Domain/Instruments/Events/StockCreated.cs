using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record StockCreated(
    StockId StockId,
    string Index,
    string Name,
    ISIN Isin,
    FIGI Figi,
    InstrumentUId InstrumentUId) : DomainEvent;