using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

public class UnitOfWorkModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WealthDbContext>(options =>
        {
            var usePostgres = configuration.GetSection("UsePostgres").Get<bool>();
            if (usePostgres)
            {
                options.UseNpgsql(configuration.GetConnectionString("CurrencyManagement"));
            }
            else
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
        });
        // services.AddMigration<WealthDbContext, FirstSeed>();

        // services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        // services.AddScoped<IOutboxRepository, OutboxRepository>();

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}