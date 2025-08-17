using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class BondDTO
{
    public BondId Id { get; set; }
    public int Quantity { get; set; }

    public static BondDTO ToDTO(BondAsset asset)
    {
        return new BondDTO
        {
            Id = asset.BondId,
            Quantity = asset.Quantity,
        };
    }
}