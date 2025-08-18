using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Operations;

public class SplitOperation : Operation
{
    public SplitRatio Ratio { get; set; }
}