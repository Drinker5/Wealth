using FluentMigrator.Runner;
using Wealth.InstrumentManagement.API.Services;
using Wealth.InstrumentManagement.Infrastructure.Dapper;

namespace Wealth.InstrumentManagement.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddLogging(l => l.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c.AddPostgres()
                .WithGlobalConnectionString(builder.Configuration.GetConnectionString("InstrumentManagement"))
                .ScanIn(AppDomain.CurrentDomain.GetAssemblies()).For.Migrations());

        builder.Services.AddHostedService<MigrationService>();

        DapperMapping.Map();
    }

    public static void MapEndpoints(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        app.MapGrpcService<InstrumentsServiceImpl>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    }
}