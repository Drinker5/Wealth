using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetStockQueryHandler(IStocksRepository repository) : IQueryHandler<GetStock, Stock?>
{
    public async Task<Stock?> Handle(GetStock request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetStock(request.Id);
        return instrument;
    }
}