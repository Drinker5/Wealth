using Wealth.BuildingBlocks.Application;

namespace Wealth.WalletManagement.API.APIs;

public class WalletServices(
    ICqrsInvoker mediator)
{
    public ICqrsInvoker Mediator { get; } = mediator;
}