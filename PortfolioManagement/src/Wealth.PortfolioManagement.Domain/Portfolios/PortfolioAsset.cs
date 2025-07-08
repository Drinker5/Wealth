namespace Wealth.PortfolioManagement.Domain.Portfolios;

public class PortfolioAsset
{
    public InstrumentId InstrumentId { get; init; }
    public ISIN ISIN { get; set; }
    public int Quantity { get; set; }

    public override int GetHashCode()
    {
        return InstrumentId.GetHashCode();
    }
}