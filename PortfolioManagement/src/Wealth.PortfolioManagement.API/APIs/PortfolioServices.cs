using Wealth.BuildingBlocks.Application;

namespace Wealth.PortfolioManagement.API.APIs;

public class PortfolioServices(
    ICqrsInvoker mediator)
{
    public ICqrsInvoker Mediator { get; } = mediator;
}