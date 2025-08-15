using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public class BondAsset
{
    public BondId BondId { get; set; }
    public ISIN ISIN { get; init; }
    public int Quantity { get; set; }
}