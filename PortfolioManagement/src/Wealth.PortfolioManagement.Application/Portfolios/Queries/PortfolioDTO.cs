using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class PortfolioDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<AssetDTO> Assets { get; set; }
    public IEnumerable<CurrencyDTO> Currencies { get; set; }

    public static PortfolioDTO ToDTO(Portfolio portfolio)
    {
        return new PortfolioDTO
        {
            Id = portfolio.Id,
            Name = portfolio.Name,
            Assets = portfolio.Assets.Select(AssetDTO.ToDTO),
            Currencies = portfolio.Currencies.Select(CurrencyDTO.ToDTO),
        };
    }
}