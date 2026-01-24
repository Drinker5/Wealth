using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Options;

namespace Wealth.InstrumentManagement.API.Extensions;

public sealed class PriceUpdaterService(
    IServiceProvider serviceProvider,
    IOptions<PriceUpdaterOptions> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var cqrsInvoker = scope.ServiceProvider.GetRequiredService<ICqrsInvoker>();

        await Task.Delay(options.Value.CheckInterval, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            await cqrsInvoker.Command(new UpdatePrices(), stoppingToken);
            await Task.Delay(options.Value.CheckInterval, stoppingToken);
        }
    }
}