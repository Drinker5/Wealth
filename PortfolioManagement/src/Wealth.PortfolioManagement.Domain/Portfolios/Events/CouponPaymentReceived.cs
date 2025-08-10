using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record CouponPaymentReceived(
    PortfolioId PortfolioId,
    BondId BondId,
    Money Income) : DomainEvent;
