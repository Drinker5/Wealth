using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Domain.Operations;

public class TradeOperation : InstrumentOperation
{
    public PortfolioId PortfolioId { get; set; }
    public Money Money { get; set; }
    public int Quantity { get; set; }
    public TradeOperationType Type { get; set; }
}

public enum TradeOperationType
{
    Buy,
    Sell
}
