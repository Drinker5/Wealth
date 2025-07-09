using Wealth.BuildingBlocks.Infrastructure.Mediation;

namespace Wealth.PortfolioManagement.API.APIs;

public class PortfolioServices(
    CqrsInvoker mediator,
    ILogger<PortfolioServices> logger)
{
    public CqrsInvoker Mediator { get; } = mediator;
    public ILogger<PortfolioServices> Logger { get; } = logger;
}