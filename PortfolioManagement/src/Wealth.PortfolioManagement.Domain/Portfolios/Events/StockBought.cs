using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record StockBought(
    PortfolioId PortfolioId,
    StockId StockId,
    Money TotalPrice,
    int Quantity) : DomainEvent;