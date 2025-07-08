using Wealth.BuildingBlocks.Domain;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public record AssetAdded(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    ISIN ISIN,
    int Quantity) : IDomainEvent;
