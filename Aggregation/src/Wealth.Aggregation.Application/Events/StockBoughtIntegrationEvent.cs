using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Events;

public record StockBoughtIntegrationEvent : IntegrationEvent
{
    public PortfolioId PortfolioId { get; set; }
    public InstrumentId InstrumentId { get; set; }
    public Money Price { get; set; }
    public int Quantity { get; set; }
}