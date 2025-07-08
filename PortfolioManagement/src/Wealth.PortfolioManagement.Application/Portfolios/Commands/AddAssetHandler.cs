using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public class AddAssetHandler(IPortfolioRepository repository) : ICommandHandler<AddAsset>
{
    public Task Handle(AddAsset request, CancellationToken cancellationToken)
    {
        return repository.AddAsset(request.PortfolioId, request.InstrumentId, request.ISIN, request.Quantity);
    }
}