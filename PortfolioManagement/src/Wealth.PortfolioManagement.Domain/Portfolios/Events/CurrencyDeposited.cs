using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record CurrencyDeposited(PortfolioId PortfolioId, CurrencyId CurrencyId, decimal Amount) : DomainEvent;