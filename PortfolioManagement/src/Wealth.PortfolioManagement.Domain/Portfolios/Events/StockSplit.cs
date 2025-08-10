
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record StockSplit(
    PortfolioId PortfolioId,
    StockId StockId,
    SplitRatio Ratio) : DomainEvent;