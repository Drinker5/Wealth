using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Wealth.BuildingBlocks.Application.Services;

namespace Wealth.PortfolioManagement.API.Tests;

public sealed class PortfolioManagementApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost app;

    public Mock<IInstrumentService> InstrumentServiceMock { get; } = new();
    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("PortfolioManagement")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private string postgresConnectionString;

    public PortfolioManagementApiFixture()
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
                { "ConnectionStrings:PortfolioManagement", postgresConnectionString },
            };
            config.AddInMemoryCollection(data!);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(InstrumentServiceMock.Object);
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