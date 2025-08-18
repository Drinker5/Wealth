using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public class BondAsset
{
    public BondId BondId { get; set; }
    public int Quantity { get; set; }
}