using Wealth.BuildingBlocks.Domain;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record TaxPaid(PortfolioId PortfolioId, InstrumentId InstrumentId, Money Expense) : IDomainEvent;