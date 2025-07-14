using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

public record StockDividendChangedIntegrationEvent : IntegrationEvent
{
    public InstrumentId InstrumentId { get; set; }
    public ISIN ISIN { get; set; }
    public Dividend NewDividend { get; set; }
}