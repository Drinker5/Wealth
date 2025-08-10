using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record StockDelisted(PortfolioId PortfolioId, StockId StockId) : DomainEvent;