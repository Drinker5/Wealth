using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record AddOperation(Operation Operation) : ICommand;