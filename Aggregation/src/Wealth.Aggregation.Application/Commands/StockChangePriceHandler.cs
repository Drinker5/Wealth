using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Commands;

public class StockChangePriceHandler(IStockAggregationRepository repository) : ICommandHandler<StockChangePrice>
{
    public Task Handle(StockChangePrice request, CancellationToken cancellationToken)
    {
        return repository.ChangePrice(request.StockId, request.NewPrice);
    }
}
