using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public sealed record StockFigiChanged(StockId StockId, FIGI Figi) : DomainEvent;