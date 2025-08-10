using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record BuyBond(
    PortfolioId PortfolioId,
    BondId BondId,
    Money TotalPrice,
    int Quantity) : ICommand;