using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Wealth.WalletManagement.API.Tests;

public sealed class WalletManagementApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost app;

    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("WalletManagement")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private string postgresConnectionString;

    public WalletManagementApiFixture()
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
                { "ConnectionStrings:WalletManagement", postgresConnectionString },
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