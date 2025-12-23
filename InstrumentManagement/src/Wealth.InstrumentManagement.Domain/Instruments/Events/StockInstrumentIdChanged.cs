using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public sealed record StockInstrumentIdChanged(StockId StockId, InstrumentId InstrumentId) : DomainEvent;