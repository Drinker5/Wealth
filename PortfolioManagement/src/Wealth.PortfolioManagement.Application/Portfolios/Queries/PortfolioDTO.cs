using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class PortfolioDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<StockDTO> Stocks { get; set; }
    public IEnumerable<BondDTO> Bonds { get; set; }
    public IEnumerable<CurrencyDTO> Currencies { get; set; }

    public static PortfolioDTO ToDTO(Portfolio portfolio)
    {
        return new PortfolioDTO
        {
            Id = portfolio.Id,
            Name = portfolio.Name,
            Bonds = portfolio.Bonds.Select(BondDTO.ToDTO),
            Stocks = portfolio.Stocks.Select(StockDTO.ToDTO),
            Currencies = portfolio.Currencies.Select(CurrencyDTO.ToDTO),
        };
    }
}