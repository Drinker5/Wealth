using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public class AddAssetHandler(IPortfolioRepository repository) : ICommandHandler<BuyAsset>
{
    public Task Handle(BuyAsset request, CancellationToken cancellationToken)
    {
        return repository.Buy(
            request.PortfolioId,
            request.InstrumentId,
            request.TotalPrice,
            request.Quantity);
    }
}