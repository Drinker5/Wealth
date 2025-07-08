using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class AssetDTO
{
    public string ISIN { get; set; }
    public int Quantity { get; set; }

    public static AssetDTO ToDTO(PortfolioAsset asset)
    {
        return new AssetDTO
        {
            ISIN = asset.ISIN,
            Quantity = asset.Quantity,
        };
    }
}