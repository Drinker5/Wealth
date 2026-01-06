using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct CreateCurrencyCommand(
    string Name,
    FIGI Figi,
    InstrumentUId InstrumentUId) : ICommand<CurrencyId>;

public class CreateCurrencyCommandHandler(ICurrenciesRepository repository) : ICommandHandler<CreateCurrencyCommand, CurrencyId>
{
    public Task<CurrencyId> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateCurrency(request, cancellationToken);
    }
}