using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record AddCurrency(PortfolioId PortfolioId, CurrencyId CurrencyId, decimal Amount) : ICommand;