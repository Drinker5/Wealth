using Wealth.BuildingBlocks.Domain;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record CurrencyWithdrew(PortfolioId PortfolioId, CurrencyId CurrencyId, decimal Amount) : IDomainEvent;