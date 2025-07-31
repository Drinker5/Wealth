using Wealth.BuildingBlocks.Application;

namespace Wealth.StrategyTracking.API.APIs;

public class StrategyTrackingServices(
    ICqrsInvoker mediator)
{
    public ICqrsInvoker Mediator { get; } = mediator;
}