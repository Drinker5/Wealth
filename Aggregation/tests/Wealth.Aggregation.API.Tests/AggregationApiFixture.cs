using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Wealth.Aggregation.API.Tests;

public sealed class AggregationApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost app;

    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("PortfolioManagement")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private string postgresConnectionString;

    public AggregationApiFixture()
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
                { "ConnectionStrings:Aggregation", postgresConnectionString },
            };
            config.AddInMemoryCollection(data!);
        });

        builder.ConfigureServices(services =>
        {
        });
        
        return base.CreateHost(builder);
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await app.StopAsync();
        app.Dispose();
        await postgresContainer.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await postgresContainer.StartAsync();
        await app.StartAsync();
        postgresConnectionString = postgresContainer.GetConnectionString();
    }
}