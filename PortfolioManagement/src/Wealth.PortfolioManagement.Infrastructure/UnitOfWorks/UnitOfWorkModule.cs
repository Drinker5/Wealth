using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;
using Wealth.PortfolioManagement.Domain.Repositories;
using Wealth.PortfolioManagement.Infrastructure.DbSeeding;
using Wealth.PortfolioManagement.Infrastructure.Repositories;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

public class UnitOfWorkModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WealthDbContext>(OptionsAction(configuration), optionsLifetime: ServiceLifetime.Singleton);
        services.AddDbContextFactory<WealthDbContext>(OptionsAction(configuration));
        services.AddMigration<WealthDbContext, DbSeeder>();
        services.AddScoped<IDbSeeder, PortfoliosSeeder>();
        services.AddScoped<IDbSeeder, PortfolioMapSeeder>();

        services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddSingleton<IOperationRepository, OperationRepository>();

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<WealthDbContext>());
    }

    private static Action<DbContextOptionsBuilder> OptionsAction(IConfiguration configuration)
    {
        return options =>
        {
            var inMemory = configuration.GetSection("InMemoryRepository").Get<bool>();
            if (inMemory)
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
            else
            {
                options.UseNpgsql(configuration.GetConnectionString("PortfolioManagement"));
                options.EnableSensitiveDataLogging();
            }
        };
    }
}