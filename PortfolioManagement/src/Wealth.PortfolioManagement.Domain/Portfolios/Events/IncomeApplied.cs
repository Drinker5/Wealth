using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public abstract record IncomeApplied(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    Money Income) : IDomainEvent;