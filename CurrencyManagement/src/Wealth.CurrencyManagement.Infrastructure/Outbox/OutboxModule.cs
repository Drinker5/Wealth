﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.Outbox;

public class OutboxModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Outbox Polling
        var outboxPollingOptions = configuration.GetSection(OutboxPollingOptions.Section).Get<OutboxPollingOptions>();
        if (outboxPollingOptions?.Enabled ?? false)
        {
            services.Configure<OutboxPollingOptions>(configuration.GetSection(OutboxPollingOptions.Section));
            services.AddSingleton<IHostedService, OutboxPollingHostedService>();
        }
    }
}