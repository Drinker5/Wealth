using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record BondSold(
    PortfolioId PortfolioId,
    BondId BondId,
    Money Price,
    int Quantity) : DomainEvent;