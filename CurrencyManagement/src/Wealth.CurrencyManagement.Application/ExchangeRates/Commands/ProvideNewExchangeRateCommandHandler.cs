using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.CurrencyManagement.Application.DataProviders;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class ProvideNewExchangeRateCommandHandler(
    IExchangeRateDataProvider exchangeRateDataProvider,
    ICommandsScheduler scheduler)
    : ICommandHandler<ProvideNewExchangeRateCommand>
{
    public async Task Handle(ProvideNewExchangeRateCommand request, CancellationToken cancellationToken)
    {
        var rate = await exchangeRateDataProvider.GetRate(request.BaseCurrency, request.TargetCurrency, request.OnDate);
        var createExchangeRateCommand = new CreateExchangeRateCommand(
            request.BaseCurrency,
            request.TargetCurrency,
            rate,
            request.OnDate);
        await scheduler.EnqueueAsync(createExchangeRateCommand, cancellationToken);
    }
}