using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeDividendCommandHandler(IStocksRepository repository) : ICommandHandler<ChangeDividendCommand>
{
    public async Task Handle(ChangeDividendCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangeDividend(request.Id, request.Dividend);
    }
}