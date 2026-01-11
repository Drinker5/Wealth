using Wealth.BuildingBlocks.Application;

namespace Wealth.InstrumentManagement.API.APIs;

public class Services(
    ICqrsInvoker mediator)
{
    public ICqrsInvoker Mediator { get; } = mediator;
}