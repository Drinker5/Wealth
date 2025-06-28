using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class CreateExchangeRateCommandHandler(
    ICurrencyRepository currencyRepository,
    IExchangeRateRepository repository) : ICommandHandler<CreateExchangeRateCommand>
{
    public async Task Handle(CreateExchangeRateCommand command, CancellationToken cancellationToken)
    {
        var baseCurrency = await currencyRepository.GetCurrency(command.BaseCurrencyId);
        var targetCurrency = await currencyRepository.GetCurrency(command.TargetCurrencyId);
        if (baseCurrency == null || targetCurrency == null)
            return;
        
        await repository.CreateExchangeRate(
            command.BaseCurrencyId,
            command.TargetCurrencyId,
            command.ExchangeRate,
            command.ValidOnDate);
    }
}