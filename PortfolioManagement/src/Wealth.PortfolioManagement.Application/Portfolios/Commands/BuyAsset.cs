using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record BuyAsset(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    Money TotalPrice,
    int Quantity) : ICommand;