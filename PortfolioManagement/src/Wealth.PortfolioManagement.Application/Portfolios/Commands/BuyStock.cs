using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record BuyStock(
    PortfolioId PortfolioId,
    StockId StockId,
    Money TotalPrice,
    int Quantity) : ICommand;