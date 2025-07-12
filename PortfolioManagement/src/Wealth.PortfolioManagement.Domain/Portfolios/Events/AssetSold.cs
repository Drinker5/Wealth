using Wealth.BuildingBlocks.Domain;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record AssetSold(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    Money Price,
    int Quantity) : IDomainEvent;