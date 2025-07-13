using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record CouponPaymentReceived(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    Money Income) : IncomeApplied(PortfolioId, InstrumentId, Income);