using System.Reflection;
using FluentMigrator.Runner;
using Wealth.InstrumentManagement.Domain.Repositories;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding.Seeds;
using Wealth.InstrumentManagement.Infrastructure.Migrations;
using Wealth.InstrumentManagement.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IInstrumentsRepository, InMemoryInstrumentRepository>();
        builder.Services.AddSingleton<IBondsRepository, InMemoryInstrumentRepository>();
        builder.Services.AddSingleton<IStocksRepository, InMemoryInstrumentRepository>();

        builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()); });

        builder.Services.AddSingleton<WealthDbContext>();
        builder.Services.AddSingleton<Database>();
        builder.Services.AddHostedService<MigrationService>();
        builder.Services.AddLogging(l => l.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c.AddPostgres()
                .WithGlobalConnectionString(builder.Configuration.GetConnectionString("InstrumentManagement"))
                .ScanIn(AppDomain.CurrentDomain.GetAssemblies()).For.Migrations());
        
        builder.Services.AddSingleton<IDbSeeder, FirstSeed>();
    }
}