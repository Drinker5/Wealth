﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.Json;

public class NewtonsoftJsonModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<JsonSerializerSettings>(c => new JsonSerializerSettings());
        services.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
    }
}