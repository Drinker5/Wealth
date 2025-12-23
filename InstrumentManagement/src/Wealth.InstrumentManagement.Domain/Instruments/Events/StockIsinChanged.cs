using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public sealed record StockIsinChanged(StockId StockId, ISIN Isin) : DomainEvent;