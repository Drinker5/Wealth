using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record StockSold(
    PortfolioId PortfolioId,
    StockId StockId,
    Money Price,
    int Quantity) : DomainEvent;