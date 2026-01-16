using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.Mediation;
using Wealth.BuildingBlocks.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding.Seeds;
using Wealth.InstrumentManagement.Infrastructure.Migrations;
using Wealth.InstrumentManagement.Infrastructure.Repositories;

namespace Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

public class UnitOfWorksModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBondsRepository, BondsRepository>();
        services.AddScoped<IStocksRepository, StocksRepository>();
        services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
        services.AddScoped<IInstrumentsRepository, InstrumentsRepository>();
        services.AddScoped<IPricesRepository, PricesRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventsResolver, WealthDbContext>();
        services.AddScoped<IConnectionFactory, WealthDbContext>();

        services.AddScoped<WealthDbContext>();
        services.AddScoped<Database>();
        services.AddScoped<IDbSeeder, FirstSeed>();

        services.AddScoped<IOutboxRepository, OutboxRepository>();
    }
}