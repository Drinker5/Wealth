using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events;

public record StockBoughtIntegrationEvent : IntegrationEvent
{
    public PortfolioId PortfolioId { get; set; }
    public InstrumentId InstrumentId { get; set; }
    public Money Price { get; set; }
    public int Quantity { get; set; }
}