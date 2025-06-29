using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeDividendCommandHandler(IStocksRepository repository, ICurrencyService currencyService) : ICommandHandler<ChangeDividendCommand>
{
    public async Task Handle(ChangeDividendCommand request, CancellationToken cancellationToken)
    {
        if (!await currencyService.IsCurrencyExists(request.Dividend.ValuePerYear.CurrencyId))
            return;

        await repository.ChangeDividend(request.Id, request.Dividend);
    }
}