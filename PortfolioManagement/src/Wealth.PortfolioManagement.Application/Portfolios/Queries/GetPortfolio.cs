using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public record GetPortfolio(PortfolioId Id) : IQuery<PortfolioDTO?>;