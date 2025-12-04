using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateStockCommandHandler : ICommandHandler<CreateStockCommand, StockId>
{
    private readonly IStocksRepository repository;

    public CreateStockCommandHandler(IStocksRepository repository)
    {
        this.repository = repository;
    }

    public Task<StockId> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateStock(
            request.Index,
            request.Name,
            request.Isin,
            request.Figi,
            request.LotSize,
            cancellationToken);
    }
}