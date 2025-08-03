using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.API.Tests;

public sealed class CurrencyManagementApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost app;
    public ICommandsScheduler CommandsScheduler = Substitute.For<ICommandsScheduler>();

    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("CurrencyManagement")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private string postgresConnectionString;

    public CurrencyManagementApiFixture()
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
                { $"ConnectionStrings:CurrencyManagement", postgresConnectionString }
            };
            config.AddInMemoryCollection(data!);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(CommandsScheduler);
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
    }
}