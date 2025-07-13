using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Operations;

public class SplitOperation : InstrumentOperation
{
    public SplitRatio Ratio { get; set; }
}