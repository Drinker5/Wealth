using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.Aggregation.Domain;
using Wealth.Aggregation.Infrastructure.DbSeeding;
using Wealth.Aggregation.Infrastructure.Repositories;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;

namespace Wealth.Aggregation.Infrastructure.UnitOfWorks;

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
                options.UseNpgsql(configuration.GetConnectionString("Aggregation"));
                options.EnableSensitiveDataLogging();
            }
        });
        services.AddMigration<WealthDbContext, FirstSeed>();

        services.AddScoped<IStockAggregationRepository, StockAggregationRepository>();

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<WealthDbContext>());
    }
}