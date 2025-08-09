using Wealth.Aggregation.Application.Services;
using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public class BuyStockHandler(IStockAggregationRepository repository, IInstrumentService instrumentService) : ICommandHandler<BuyStock>
{
    public async Task Handle(BuyStock request, CancellationToken cancellationToken)
    {
        var stockAggregation = await repository.GetStock(request.StockId);
        if (stockAggregation is null)
            await CreateStock(request.StockId);

        await repository.Buy(request.StockId, request.Quantity, request.TotalPrice);
    }

    private async Task CreateStock(StockId stockId)
    {
        var instrumentInfo = await instrumentService.GetStockInfo(stockId);
        if (instrumentInfo == null)
            return;
            
        await repository.Create(
            stockId,
            instrumentInfo.Name,
            instrumentInfo.Price,
            instrumentInfo.DividendPerYear,
            instrumentInfo.LotSize);
    }
}