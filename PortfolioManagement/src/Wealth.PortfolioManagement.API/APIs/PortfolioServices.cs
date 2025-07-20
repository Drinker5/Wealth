using Wealth.BuildingBlocks.Application;

namespace Wealth.PortfolioManagement.API.APIs;

public class PortfolioServices(
    ICqrsInvoker mediator,
    ILogger<PortfolioServices> logger)
{
    public ICqrsInvoker Mediator { get; } = mediator;
    public ILogger<PortfolioServices> Logger { get; } = logger;
}