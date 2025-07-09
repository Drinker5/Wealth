using Wealth.BuildingBlocks.Domain;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record PortfolioRenamed(PortfolioId Id, string NewName) : IDomainEvent;