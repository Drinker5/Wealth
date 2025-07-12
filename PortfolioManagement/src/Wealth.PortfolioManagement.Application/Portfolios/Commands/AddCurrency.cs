using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record AddCurrency(PortfolioId PortfolioId, Money Money) : ICommand;