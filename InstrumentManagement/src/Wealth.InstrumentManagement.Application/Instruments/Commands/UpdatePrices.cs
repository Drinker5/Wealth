using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public record struct UpdatePrices : ICommand;

public class UpdatePricesHandler(IPriceUpdater priceUpdater) : ICommandHandler<UpdatePrices>
{
    public Task Handle(UpdatePrices request, CancellationToken cancellationToken)
    {
        return priceUpdater.UpdatePrices(cancellationToken);
    }
}