using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record BondBought(
    PortfolioId PortfolioId,
    BondId BondId,
    Money TotalPrice,
    int Quantity) : DomainEvent;