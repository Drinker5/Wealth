using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Domain.Interfaces;
using Wealth.CurrencyManagement.Domain.Repositories;
using Wealth.CurrencyManagement.Infrastructure.Repositories;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWork.Decorators;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWork;

public static class UnitOfWorkModule
{
    public static void RegisterUnitOfWorkModule(this IServiceCollection services)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        dbContextOptionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        services.AddScoped(c => new WealthDbContext(dbContextOptionsBuilder.Options));

        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Decorate<IUnitOfWork, LoggingUnitOfWorkDecorator>();
    }
}