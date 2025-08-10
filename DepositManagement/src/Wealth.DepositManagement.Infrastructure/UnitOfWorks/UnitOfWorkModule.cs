using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Wealth.DepositManagement.Domain.Repositories;
using Wealth.DepositManagement.Infrastructure.DbSeeding;
using Wealth.DepositManagement.Infrastructure.Repositories;

namespace Wealth.DepositManagement.Infrastructure.UnitOfWorks;

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
                options.UseNpgsql(configuration.GetConnectionString("DepositManagement"));
                options.EnableSensitiveDataLogging();
            }
        });
        services.AddMigration<WealthDbContext, FirstSeed>();

        services.AddScoped<IDepositRepository, DepositRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IDepositOperationRepository, DepositOperationRepository>();

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<WealthDbContext>());
    }
}