using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public record BuyAsset(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    Money TotalPrice,
    int Quantity) : ICommand;