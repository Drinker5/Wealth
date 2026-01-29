using FluentMigrator.Runner;
using Wealth.InstrumentManagement.API.APIs;
using Wealth.InstrumentManagement.API.Services;
using Wealth.InstrumentManagement.Infrastructure.Dapper;

namespace Wealth.InstrumentManagement.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddLogging(l => l.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c.AddPostgres()
                .WithGlobalConnectionString(builder.Configuration.GetConnectionString("InstrumentManagement"))
                .ScanIn(AppDomain.CurrentDomain.GetAssemblies()).For.Migrations());

        builder.Services.AddHostedService<MigrationService>();
        builder.Services.AddHostedService<PriceUpdaterService>();
        DapperMapping.Map();
    }

    public static void MapEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapInstrumentsApi();
        app.MapGrpcService<InstrumentsServiceImpl>();
    }
}