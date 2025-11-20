using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class CreateExchangeRateCommandHandler(
    IExchangeRateRepository repository) : ICommandHandler<CreateExchangeRateCommand>
{
    public async Task Handle(CreateExchangeRateCommand command, CancellationToken cancellationToken)
    {
        await repository.CreateExchangeRate(
            command.BaseCurrency,
            command.TargetCurrency,
            command.ExchangeRate,
            command.ValidOnDate);
    }
}