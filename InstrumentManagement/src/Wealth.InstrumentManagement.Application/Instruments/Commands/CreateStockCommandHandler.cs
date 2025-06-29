using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateStockCommandHandler : ICommandHandler<CreateStockCommand>
{
    private readonly IStocksRepository repository;

    public CreateStockCommandHandler(IStocksRepository repository)
    {
        this.repository = repository;
    }

    public Task Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateStock(request.Name, request.ISIN);
    }
}