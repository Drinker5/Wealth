using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Portfolios.Events;

public record CurrencyDepositedIntegrationEvent : IntegrationEvent
{
    public PortfolioId PortfolioId { get; set; }
    public CurrencyId CurrencyId { get; set; }
    public decimal Amount { get; set; }

    public override string Key => PortfolioId.ToString();
}