using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events;

public record BondBoughtIntegrationEvent : IntegrationEvent
{
    public PortfolioId PortfolioId { get; set; }
    public InstrumentId InstrumentId { get; set; }
    public Money Price { get; set; }
    public int Quantity { get; set; }
}