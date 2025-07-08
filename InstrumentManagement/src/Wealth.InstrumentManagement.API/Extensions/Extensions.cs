using FluentMigrator.Runner;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Repositories;
using Wealth.InstrumentManagement.Infrastructure.Dapper;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding.Seeds;
using Wealth.InstrumentManagement.Infrastructure.Mediation.CommandBehaviors;
using Wealth.InstrumentManagement.Infrastructure.Migrations;
using Wealth.InstrumentManagement.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        if (builder.Configuration.GetValue<bool>("InMemoryRepository"))
        {
            builder.Services.AddSingleton<IInstrumentsRepository, InMemoryInstrumentRepository>();
            builder.Services.AddSingleton<IBondsRepository, InMemoryInstrumentRepository>();
            builder.Services.AddSingleton<IStocksRepository, InMemoryInstrumentRepository>();
        }
        else
        {
            builder.Services.AddScoped<IInstrumentsRepository, InstrumentRepository>();
            builder.Services.AddScoped<IBondsRepository, InstrumentRepository>();
            builder.Services.AddScoped<IStocksRepository, InstrumentRepository>();
        }
        builder.Services.AddScoped<UnitOfWork>();
        
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.AddOpenBehavior(typeof(CommandUnitOfWorkBehavior<,>));
            cfg.AddOpenBehavior(typeof(CommandEventDispatchBehavior<,>));
        });

        builder.Services.AddScoped<WealthDbContext>();
        builder.Services.AddScoped<Database>();
        builder.Services.AddHostedService<MigrationService>();
        builder.Services.AddLogging(l => l.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c.AddPostgres()
                .WithGlobalConnectionString(builder.Configuration.GetConnectionString("InstrumentManagement"))
                .ScanIn(AppDomain.CurrentDomain.GetAssemblies()).For.Migrations());

        builder.Services.AddScoped<IDbSeeder, FirstSeed>();

        DapperMapping.Map();
        
        builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
    }
}