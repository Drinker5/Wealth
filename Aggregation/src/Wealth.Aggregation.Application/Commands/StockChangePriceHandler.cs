using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Commands;

public class StockChangePriceHandler(IStockTradeOperationRepository operationRepository) : ICommandHandler<StockChangePrice>
{
    public Task Handle(StockChangePrice request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //return repository.ChangePrice(request.StockId, request.NewPrice);
    }
}
