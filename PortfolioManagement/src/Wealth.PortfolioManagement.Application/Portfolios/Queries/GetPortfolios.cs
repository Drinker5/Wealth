using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public record GetPortfolios() : IQuery<IEnumerable<PortfolioDTO>>;

public class GetPortfoliosHandler(IPortfolioRepository repository) : IQueryHandler<GetPortfolios, IEnumerable<PortfolioDTO>>
{
    public async Task<IEnumerable<PortfolioDTO>> Handle(GetPortfolios request, CancellationToken cancellationToken)
    {
        var portfolios = await repository.GetPortfolios();
        return portfolios.Select(PortfolioDTO.ToDTO);
    }
}