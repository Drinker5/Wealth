using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class CreateExchangeRateCommandHandler(IExchangeRateRepository repository) : ICommandHandler<CreateExchangeRateCommand>
{
    public Task Handle(CreateExchangeRateCommand command, CancellationToken cancellationToken)
    {
        return repository.CreateExchangeRate(
            command.BaseCurrencyId,
            command.TargetCurrencyId,
            command.ExchangeRate,
            command.ValidOnDate);
    }
}