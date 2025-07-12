using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record AmortizationApplied(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    Money Income) : IncomeApplied(PortfolioId, InstrumentId, Income);