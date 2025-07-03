using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateStockCommandHandler : ICommandHandler<CreateStockCommand, InstrumentId>
{
    private readonly IStocksRepository repository;

    public CreateStockCommandHandler(IStocksRepository repository)
    {
        this.repository = repository;
    }

    public Task<InstrumentId> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateStock(request.Name, request.ISIN);
    }
}