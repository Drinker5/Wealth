using Wealth.BuildingBlocks.Domain;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record PortfolioCreated(PortfolioId PortfolioId, string Name) : IDomainEvent;
