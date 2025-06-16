using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Infrastructure.Json;

public static class NewtonsoftJsonModule
{
    public static void RegisterNewtonsoftJsonModule(this IServiceCollection services)
    {
        services.AddSingleton<JsonSerializerSettings>(c => new JsonSerializerSettings());
        services.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
    }
}