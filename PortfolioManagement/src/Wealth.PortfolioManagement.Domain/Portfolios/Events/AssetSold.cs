using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record AssetSold(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    Money Price,
    int Quantity) : DomainEvent;