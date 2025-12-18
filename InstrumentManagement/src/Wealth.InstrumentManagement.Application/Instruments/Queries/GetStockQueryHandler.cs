using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetStockQueryHandler(IStocksRepository repository) :
    IQueryHandler<GetStock, Stock?>,
    IQueryHandler<GetStockByIsin, Stock?>,
    IQueryHandler<GetStockByFigi, Stock?>,
    IQueryHandler<GetStocksQuery, IReadOnlyCollection<Stock>>
{
    public async Task<Stock?> Handle(GetStock request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetStock(request.Id);
        return instrument;
    }

    public async Task<Stock?> Handle(GetStockByIsin request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetStock(request.Isin);
        return instrument;
    }

    public async Task<Stock?> Handle(GetStockByFigi request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetStock(request.Figi);
        return instrument;
    }

    public async Task<IReadOnlyCollection<Stock>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
    {
        var stocks = await repository.GetStocks();
        return stocks;
    }
}