using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record AddCurrency(PortfolioId PortfolioId, Money Money) : ICommand;