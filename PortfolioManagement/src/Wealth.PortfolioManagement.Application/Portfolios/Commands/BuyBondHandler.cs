using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public class BuyBondHandler(IPortfolioRepository repository) : ICommandHandler<BuyBond>
{
    public Task Handle(BuyBond request, CancellationToken cancellationToken)
    {
        return repository.BuyBond(
            request.PortfolioId,
            request.BondId,
            request.TotalPrice,
            request.Quantity);
    }
}