using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class AssetDTO
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }

    public static AssetDTO ToDTO(PortfolioAsset asset)
    {
        return new AssetDTO
        {
            Id = asset.InstrumentId,
            Quantity = asset.Quantity,
        };
    }
}