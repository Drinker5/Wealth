using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record TaxPaid(PortfolioId PortfolioId, InstrumentId InstrumentId, Money Expense) : DomainEvent;