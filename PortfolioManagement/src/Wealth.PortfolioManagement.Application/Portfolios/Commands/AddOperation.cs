using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public readonly record struct AddOperation(Operation Operation) : ICommand;