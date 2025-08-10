using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.CurrencyManagement.Application.DataProviders;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class ProvideNewExchangeRateCommandHandler : ICommandHandler<ProvideNewExchangeRateCommand>
{
    private readonly IExchangeRateDataProvider exchangeRateDataProvider;
    private readonly ICommandsScheduler scheduler;

    public ProvideNewExchangeRateCommandHandler(
        IExchangeRateDataProvider exchangeRateDataProvider,
        ICommandsScheduler scheduler)
    {
        this.exchangeRateDataProvider = exchangeRateDataProvider;
        this.scheduler = scheduler;
    }

    public async Task Handle(ProvideNewExchangeRateCommand request, CancellationToken cancellationToken)
    {
        var rate = await exchangeRateDataProvider.GetRate(request.BaseCurrencyId, request.TargetCurrencyId, request.OnDate);
        var createExchangeRateCommand = new CreateExchangeRateCommand(request.BaseCurrencyId, request.TargetCurrencyId, rate, request.OnDate);
        await scheduler.EnqueueAsync(createExchangeRateCommand, cancellationToken);
    }
}