using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateCurrencyCommandHandler(ICurrenciesRepository repository) : ICommandHandler<CreateCurrencyCommand, CurrencyId>
{
    public Task<CurrencyId> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateCurrency(request, cancellationToken);
    }
}