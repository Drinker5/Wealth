using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Domain.Tests.TestHelpers;

public class PortfolioBuilder
{
    private string name = "Foo";

    public Portfolio Build()
    {
        return Portfolio.Create(name);
    }

    public PortfolioBuilder SetName(string newName)
    {
        this.name = newName;
        return this;
    }
}