using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public class BuyStockHandler(IPortfolioRepository repository) : ICommandHandler<BuyStock>
{
    public Task Handle(BuyStock request, CancellationToken cancellationToken)
    {
        return repository.BuyStock(
            request.PortfolioId,
            request.StockId,
            request.TotalPrice,
            request.Quantity);
    }
}