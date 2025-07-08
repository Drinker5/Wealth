using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record AddAsset(PortfolioId PortfolioId, InstrumentId InstrumentId, ISIN ISIN, int Quantity) : ICommand;