using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record DividendReceived(
    PortfolioId PortfolioId,
    StockId StockId,
    Money Income) : DomainEvent;