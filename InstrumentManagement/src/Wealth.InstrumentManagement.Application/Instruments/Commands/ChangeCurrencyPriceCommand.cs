using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct ChangeCurrencyPriceCommand(CurrencyId CurrencyId, Money Price) : ICommand;

public class ChangeCurrencyPriceCommandHandler(ICurrenciesRepository repository) : ICommandHandler<ChangeCurrencyPriceCommand>
{
    public async Task Handle(ChangeCurrencyPriceCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangePrice(request.CurrencyId, request.Price);
    }
}