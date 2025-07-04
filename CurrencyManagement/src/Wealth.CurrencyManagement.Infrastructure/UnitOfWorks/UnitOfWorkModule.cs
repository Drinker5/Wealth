﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Domain;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Repositories;
using Wealth.CurrencyManagement.Infrastructure.Abstractions;
using Wealth.CurrencyManagement.Infrastructure.DbSeeding.Seeds;
using Wealth.CurrencyManagement.Infrastructure.Extensions;
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
                options.UseNpgsql(configuration.GetConnectionString("CurrencyManagement"));
            }
            else
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
        });
        services.AddMigration<WealthDbContext, FirstSeed>(); 

        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Decorate<IUnitOfWork, UnitOfWorkLoggingDecorator>();
        services.Decorate<IUnitOfWork, UnitOfWorkCreateOutboxMessagesDecorator>();
    }
}