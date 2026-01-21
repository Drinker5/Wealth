using Microsoft.Extensions.Options;
using Wealth.InstrumentManagement.Application.Options;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.API.Extensions;

public sealed class PriceUpdaterService(
    IServiceProvider serviceProvider,
    IOptions<PriceUpdaterOptions> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var priceUpdater = scope.ServiceProvider.GetRequiredService<IPriceUpdater>();

        await Task.Delay(options.Value.CheckInterval, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO mediator
            await priceUpdater.UpdatePrices(stoppingToken);
            await Task.Delay(options.Value.CheckInterval, stoppingToken);
        }
    }
}