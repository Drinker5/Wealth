using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record StockOperationTaxPaid(PortfolioId PortfolioId, StockId StockId, Money Expense) : DomainEvent;