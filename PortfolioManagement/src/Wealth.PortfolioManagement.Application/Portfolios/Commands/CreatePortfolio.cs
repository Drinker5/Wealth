using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record CreatePortfolio(string Name) : ICommand<PortfolioId>;