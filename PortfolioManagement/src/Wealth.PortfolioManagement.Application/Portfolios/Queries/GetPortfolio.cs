using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public record GetPortfolio(PortfolioId Id) : IQuery<PortfolioDTO?>;