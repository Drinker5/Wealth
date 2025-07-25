﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Repositories;
using Wealth.CurrencyManagement.Infrastructure.DbSeeding;
using Wealth.CurrencyManagement.Infrastructure.Repositories;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.Decorators;
using Wealth.DepositManagement.Infrastructure.Repositories;

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

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Decorate<IUnitOfWork, UnitOfWorkLoggingDecorator>();
        services.Decorate<IUnitOfWork, UnitOfWorkCreateOutboxMessagesDecorator>();
    }
}