using Wealth.BuildingBlocks.Domain;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record AssetBought(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    Money Price,
    int Quantity) : IDomainEvent;