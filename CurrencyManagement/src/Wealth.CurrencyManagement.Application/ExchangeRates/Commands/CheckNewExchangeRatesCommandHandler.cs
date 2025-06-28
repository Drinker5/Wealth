using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class CheckNewExchangeRatesCommandHandler : ICommandHandler<CheckNewExchangeRatesCommand>
{
    private readonly ICurrencyRepository currencyRepository;
    private readonly IExchangeRateRepository exchangeRateRepository;
    private readonly ICommandsScheduler scheduler;

    public CheckNewExchangeRatesCommandHandler(
        ICurrencyRepository currencyRepository,
        IExchangeRateRepository exchangeRateRepository,
        ICommandsScheduler scheduler)
    {
        this.currencyRepository = currencyRepository;
        this.exchangeRateRepository = exchangeRateRepository;
        this.scheduler = scheduler;
    }

    public async Task Handle(CheckNewExchangeRatesCommand request, CancellationToken cancellationToken)
    {
        var c1 = await currencyRepository.GetCurrency(request.FromCurrency);
        var c2 = await currencyRepository.GetCurrency(request.ToCurrency);
        if (c1 == null)
            throw new InvalidOperationException($"Currency {request.FromCurrency} not found");

        if (c2 == null)
            throw new InvalidOperationException($"Currency {request.ToCurrency} not found");

        if (c1 == c2)
            throw new InvalidOperationException($"Currency {request.ToCurrency} is same");
        
        var date = await exchangeRateRepository.GetLastExchangeRateDate(request.FromCurrency, request.ToCurrency);

        var scheduled = 0;
        for (var d = date.AddDays(1); d <= Clock.Today; d = d.AddDays(1))
        {
            // want 10 per minute, 1 per 6 second
            var whenToExecute = Clock.Now.AddSeconds(6 * scheduled);
            var provideNewExchangeRateCommand = new ProvideNewExchangeRateCommand(request.FromCurrency, request.ToCurrency, d);
            await scheduler.ScheduleAsync(provideNewExchangeRateCommand, whenToExecute, cancellationToken);
            scheduled++;
            if (scheduled > 30) // schedule only 30 per command
                break;
        }
    }
}