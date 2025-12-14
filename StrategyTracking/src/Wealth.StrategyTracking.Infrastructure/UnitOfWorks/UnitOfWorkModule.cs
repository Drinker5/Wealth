using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;
using Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;
using Wealth.StrategyTracking.Domain.Repositories;
using Wealth.StrategyTracking.Infrastructure.DbSeeding;
using Wealth.StrategyTracking.Infrastructure.Repositories;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks;

public class UnitOfWorkModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WealthDbContext>(options =>
        {
            var inMemory = configuration.GetSection("InMemoryRepository").Get<bool>();
            if (inMemory)
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
            else
            {
                options.UseNpgsql(configuration.GetConnectionString("StrategyTracking"), builder =>
                {
                    builder.EnableRetryOnFailure(5);
                });
                options.EnableSensitiveDataLogging();
            }
        });
        services.AddMigration<WealthDbContext, FirstSeed>();

        services.AddScoped<IStrategyRepository, StrategyRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<WealthDbContext>());

        services.AddSingleton<ComponentsProviderFactory>();
        services.AddSingleton<IMoexComponentsProvider, MoexComponentProvider>();
    }
}