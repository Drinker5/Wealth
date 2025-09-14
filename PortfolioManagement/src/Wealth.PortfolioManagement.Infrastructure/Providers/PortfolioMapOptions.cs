namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public class PortfolioMapOptions
{
    public const string Section = "PortfolioMap";

    public Dictionary<string, int> PortfolioIdMap { get; set; } = [];
}