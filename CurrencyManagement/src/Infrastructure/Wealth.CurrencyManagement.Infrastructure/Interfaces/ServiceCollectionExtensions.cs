using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wealth.CurrencyManagement.Infrastructure.Interfaces;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceModules(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var moduleTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IServiceModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var moduleType in moduleTypes)
        {
            if (Activator.CreateInstance(moduleType) is IServiceModule module)
                module.ConfigureServices(services, configuration);
        }

        return services;
    }
}