using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class StockDTO
{
    public StockId Id { get; set; }
    public int Quantity { get; set; }

    public static StockDTO ToDTO(StockAsset asset)
    {
        return new StockDTO
        {
            Id = asset.StockId,
            Quantity = asset.Quantity,
        };
    }
}