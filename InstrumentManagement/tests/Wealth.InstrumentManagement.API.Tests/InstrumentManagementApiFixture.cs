﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Testcontainers.PostgreSql;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Infrastructure.Services;
using Xunit;

namespace Wealth.InstrumentManagement.API.Tests;

public sealed class InstrumentManagementApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost app;
    public ICurrencyService CurrencyService = A.Fake<ICurrencyService>();

    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("InstrumentManagement")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private string postgresConnectionString;
    private string masterConnectionString;

    public InstrumentManagementApiFixture()
    {
        var appBuilder = new HostApplicationBuilder();
        
        app = appBuilder.Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            var data = new Dictionary<string, string>
            {
                { "ConnectionStrings:InstrumentManagement", postgresConnectionString },
                { "ConnectionStrings:Master", masterConnectionString },
            };
            config.AddInMemoryCollection(data!);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(CurrencyService);
        });
        return base.CreateHost(builder);
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await app.StopAsync();
        if (app is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            app.Dispose();
        }
        
        await postgresContainer.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await postgresContainer.StartAsync();
        await app.StartAsync();
        postgresConnectionString = postgresContainer.GetConnectionString();
        masterConnectionString = new NpgsqlConnectionStringBuilder(postgresConnectionString)
        {
            Database = "postgres"
        }.ToString();
    }
}