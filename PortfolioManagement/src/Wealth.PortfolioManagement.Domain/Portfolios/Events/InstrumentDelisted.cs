using Wealth.BuildingBlocks.Domain;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record InstrumentDelisted(PortfolioId PortfolioId, InstrumentId InstrumentId) : IDomainEvent;