using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record AmortizationApplied(
    PortfolioId PortfolioId,
    BondId BondId,
    Money Income);