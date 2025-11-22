using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class CheckNewExchangeRatesCommandHandler(
    IExchangeRateRepository exchangeRateRepository,
    ICommandsScheduler scheduler)
    : ICommandHandler<CheckNewExchangeRatesCommand>
{
    public async Task Handle(CheckNewExchangeRatesCommand request, CancellationToken cancellationToken)
    {
        var c1 = request.FromCurrency;
        var c2 = request.ToCurrency;

        if (c1 == CurrencyCode.None)
            throw new InvalidOperationException($"Currency {request.FromCurrency} not found");

        if (c2 == CurrencyCode.None)
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
            if (scheduled >= 30) // schedule only 30 per command
                break;
        }
    }
}