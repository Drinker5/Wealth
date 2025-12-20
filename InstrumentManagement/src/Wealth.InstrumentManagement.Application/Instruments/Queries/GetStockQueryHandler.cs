using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetStockQueryHandler(IStocksRepository repository) :
    IQueryHandler<GetStock, Stock?>,
    IQueryHandler<GetStockByIsin, Stock?>,
    IQueryHandler<GetStockByFigi, Stock?>,
    IQueryHandler<GetStockByInstrumentId, Stock?>,
    IQueryHandler<GetStocksQuery, IReadOnlyCollection<Stock>>
{
    public Task<Stock?> Handle(GetStock request, CancellationToken cancellationToken)
    {
        return repository.GetStock(request.Id);
    }

    public Task<Stock?> Handle(GetStockByIsin request, CancellationToken cancellationToken)
    {
        return repository.GetStock(request.Isin);
    }

    public Task<Stock?> Handle(GetStockByFigi request, CancellationToken cancellationToken)
    {
        return repository.GetStock(request.Figi);
    }

    public Task<Stock?> Handle(GetStockByInstrumentId request, CancellationToken cancellationToken)
    {
        return repository.GetStock(request.Id);
    }

    public async Task<IReadOnlyCollection<Stock>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
    {
        var stocks = await repository.GetStocks();
        return stocks;
    }
}