using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Application.Services;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public class BuyStockHandler(IStockAggregationRepository repository, IInstrumentService instrumentService) : ICommandHandler<BuyStock>
{
    public async Task Handle(BuyStock request, CancellationToken cancellationToken)
    {
        var stockAggregation = await repository.GetStock(request.InstrumentId);
        if (stockAggregation is null)
            await CreateStock(request.InstrumentId);

        await repository.Buy(request.InstrumentId, request.Quantity, request.TotalPrice);
    }

    private async Task CreateStock(InstrumentId instrumentId)
    {
        var instrumentInfo = await instrumentService.GetInstrumentInfo(instrumentId) as StockInstrumentInfo;
        if (instrumentInfo == null)
            return;
            
        await repository.Create(
            instrumentId,
            instrumentInfo.Name,
            instrumentInfo.Price,
            instrumentInfo.DividendPerYear,
            instrumentInfo.LotSize);
    }
}