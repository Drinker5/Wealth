using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class GetPortfolioHandler(IPortfolioRepository repository) : IQueryHandler<GetPortfolio, PortfolioDTO?>
{
    public async Task<PortfolioDTO?> Handle(GetPortfolio request, CancellationToken cancellationToken)
    {
        var portfolio = await repository.GetPortfolio(request.Id);
        if (portfolio == null)
            return null;

        return new PortfolioDTO
        {
            Id = portfolio.Id,
            Name = portfolio.Name,
            Assets = portfolio.Assets.Select(AssetDTO.ToDTO),
            Currencies = portfolio.Currencies.Select(CurrencyDTO.ToDTO),
        };
    }
}