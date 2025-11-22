using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Commands;

public class StockChangePriceHandler(IStockTradeRepository repository) : ICommandHandler<StockChangePrice>
{
    public Task Handle(StockChangePrice request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //return repository.ChangePrice(request.StockId, request.NewPrice);
    }
}
