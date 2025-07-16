using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Application.Events;

public record CurrencyDepositedIntegrationEvent : IntegrationEvent
{
    public PortfolioId PortfolioId { get; set; }
    public CurrencyId CurrencyId { get; set; }
    public decimal Amount { get; set; }

    public override string Key => PortfolioId.ToString();
}