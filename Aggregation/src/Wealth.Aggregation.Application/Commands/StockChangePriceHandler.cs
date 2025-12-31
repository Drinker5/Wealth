using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Commands;

public class StockChangePriceHandler : ICommandHandler<StockChangePrice>
{
    public Task Handle(StockChangePrice request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //return repository.ChangePrice(request.StockId, request.NewPrice);
    }
}
