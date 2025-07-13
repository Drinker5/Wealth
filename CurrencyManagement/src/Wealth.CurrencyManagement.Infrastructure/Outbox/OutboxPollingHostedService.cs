using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wealth.BuildingBlocks.Infrastructure.Mediation;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.Outbox.Commands;

namespace Wealth.CurrencyManagement.Infrastructure.Outbox;

public class OutboxPollingHostedService : IHostedService
{
    private readonly ILogger<OutboxPollingHostedService> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly CqrsInvoker invoker;

    public OutboxPollingHostedService(
        ILogger<OutboxPollingHostedService> logger,
        IServiceProvider serviceProvider,
        CqrsInvoker invoker)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.invoker = invoker;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting Outbox Polling on a background thread. '{Section}.Enabled = true'", OutboxPollingOptions.Section);
        _ = Task.Run(() => DoStuffPeriodically(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task DoStuffPeriodically(CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IDeferredOperationRepository>();
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            var unprocessed = await repository.LoadUnprocessed(1, cancellationToken);
            foreach (var outboxMessageId in unprocessed)
                await invoker.Command(new ProcessOutboxCommand(outboxMessageId));
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}