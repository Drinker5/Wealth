using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Providers;

public interface IPortfolioIdProvider
{
    ValueTask<PortfolioId> GetPortfolioIdByAccountId(string accountId, CancellationToken token = default);
}

public class PortfolioIdMap
{
    public string AccountId { get; set; }
    public PortfolioId PortfolioId { get; set; }
}