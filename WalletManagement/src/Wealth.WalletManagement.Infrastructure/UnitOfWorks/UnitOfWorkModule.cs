using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;
using Wealth.WalletManagement.Domain.Repositories;
using Wealth.WalletManagement.Infrastructure.DbSeeding;
using Wealth.WalletManagement.Infrastructure.Repositories;

namespace Wealth.WalletManagement.Infrastructure.UnitOfWorks;

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
                options.UseNpgsql(configuration.GetConnectionString("WalletManagement"));
                options.EnableSensitiveDataLogging();
            }
        });
        services.AddMigration<WealthDbContext, FirstSeed>();

        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IWalletOperationRepository, WalletOperationRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<WealthDbContext>());
    }
}