using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Domain.Interfaces;
using Wealth.CurrencyManagement.Domain.Repositories;
using Wealth.CurrencyManagement.Infrastructure.Interfaces;
using Wealth.CurrencyManagement.Infrastructure.Repositories;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.Decorators;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

public class UnitOfWorkModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WealthDbContext>(options =>
        {
            var usePostgress = configuration.GetSection("UsePostgres").Get<bool>();
            if (usePostgress)
            {
                options.UseNpgsql(configuration.GetConnectionString("Wealth.CurrencyManagement"));
            }
            else
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
        });

        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Decorate<IUnitOfWork, LoggingUnitOfWorkDecorator>();
    }
}