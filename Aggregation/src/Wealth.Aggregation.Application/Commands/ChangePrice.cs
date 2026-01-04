using Wealth.Aggregation.Application.Models;
using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Commands;

public record ChangePrice(IReadOnlyCollection<InstrumentPrice> NewPrices) : ICommand;

public class InstrumentChangePriceHandler(IPriceRepository repository) : ICommandHandler<ChangePrice>
{
    public Task Handle(ChangePrice request, CancellationToken cancellationToken)
    {
        return repository.ChangePrices(request.NewPrices, cancellationToken);
    }
}