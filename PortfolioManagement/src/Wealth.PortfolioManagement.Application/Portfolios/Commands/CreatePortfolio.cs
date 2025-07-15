using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record CreatePortfolio(string Name) : ICommand<PortfolioId>;