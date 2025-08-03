using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;
using Wealth.CurrencyManagement.Domain.Repositories;
using Wealth.CurrencyManagement.Infrastructure.DbSeeding;
using Wealth.CurrencyManagement.Infrastructure.Repositories;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

public class UnitOfWorkModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WealthDbContext>(options =>
        {
            var usePostgres = configuration.GetSection("UsePostgres").Get<bool>();
            if (usePostgres)
            {
                options.UseNpgsql(configuration.GetConnectionString("CurrencyManagement"), builder =>
                {
                    builder.EnableRetryOnFailure(5);
                });
            }
            else
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
        });

        services.AddMigration<WealthDbContext, FirstSeed>(); 

        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddScoped<IDeferredOperationRepository, DeferredOperationRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<WealthDbContext>());
    }
}