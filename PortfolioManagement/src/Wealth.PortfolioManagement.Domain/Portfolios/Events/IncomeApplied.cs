using Wealth.BuildingBlocks.Domain;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public abstract record IncomeApplied(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    Money Income) : IDomainEvent;