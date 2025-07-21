using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record CurrencyWithdrew(PortfolioId PortfolioId, CurrencyId CurrencyId, decimal Amount) : DomainEvent;