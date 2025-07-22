using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Wealth.CurrencyManagement.API.Tests;

public sealed class CurrencyManagementApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost app;

    public IResourceBuilder<PostgresServerResource> Postgres { get; private set; }
    private string postgresConnectionString;

    public CurrencyManagementApiFixture()
    {
        var options = new DistributedApplicationOptions
        {
            AssemblyName = typeof(CurrencyManagementApiFixture).Assembly.FullName,
            DisableDashboard = true
        };
        var appBuilder = DistributedApplication.CreateBuilder(options);
        Postgres = appBuilder.AddPostgres("CurrencyManagement")
            .WithImage("ankane/pgvector")
            .WithImageTag("latest");
        
        app = appBuilder.Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            var data = new Dictionary<string, string>
            {
                { $"ConnectionStrings:{Postgres.Resource.Name}", postgresConnectionString }
            };
            config.AddInMemoryCollection(data!);
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
    }

    public async Task InitializeAsync()
    {
        await app.StartAsync();
        postgresConnectionString = await Postgres.Resource.GetConnectionStringAsync() ?? throw new InvalidOperationException();
    }
}