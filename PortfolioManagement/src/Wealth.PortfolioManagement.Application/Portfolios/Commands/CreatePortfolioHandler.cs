using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public class CreatePortfolioHandler(IPortfolioRepository repository) : ICommandHandler<CreatePortfolio, PortfolioId>
{
    public Task<PortfolioId> Handle(CreatePortfolio request, CancellationToken cancellationToken)
    {
        return repository.CreatePortfolio(request.Name);
    }
}