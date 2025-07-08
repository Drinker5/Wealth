using Wealth.BuildingBlocks.Domain;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record CurrencyAdded(PortfolioId PortfolioId, CurrencyId CurrencyId, decimal Amount) : IDomainEvent;