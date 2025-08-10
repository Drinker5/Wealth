using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.Mediation;
using Wealth.InstrumentManagement.Domain.Repositories;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding.Seeds;
using Wealth.InstrumentManagement.Infrastructure.Migrations;
using Wealth.InstrumentManagement.Infrastructure.Repositories;

namespace Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

public class UnitOfWorksModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("InMemoryRepository"))
        {
            services.AddSingleton<IBondsRepository, InMemoryInstrumentRepository>();
            services.AddSingleton<IStocksRepository, InMemoryInstrumentRepository>();
        }
        else
        {
            services.AddScoped<IBondsRepository, InstrumentRepository>();
            services.AddScoped<IStocksRepository, InstrumentRepository>();
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventsResolver, WealthDbContext>();

        services.AddScoped<WealthDbContext>();
        services.AddScoped<Database>();
        services.AddScoped<IDbSeeder, FirstSeed>();

        services.AddScoped<IOutboxRepository, OutboxRepository>();
    }
}