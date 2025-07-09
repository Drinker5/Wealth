using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wealth.BuildingBlocks.Infrastructure;

namespace Wealth.BuildingBlocks.API;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddServiceModules(this IHostApplicationBuilder builder)
    {
        AddServiceModules(builder.Services, builder.Configuration);
        return builder;
    }

    private static IServiceCollection AddServiceModules(IServiceCollection services, IConfiguration configuration)
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        if (string.IsNullOrEmpty(assemblyDirectory))
            throw new InvalidOperationException("Could not determine assembly directory.");

        var dllFiles = Directory.GetFiles(assemblyDirectory, $"{nameof(Wealth)}.*.dll", SearchOption.TopDirectoryOnly);
        var assemblies = new List<Assembly>();
        foreach (var dllFile in dllFiles)
        {
            var assemblyName = AssemblyName.GetAssemblyName(dllFile);
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
            assemblies.Add(assembly);
        }

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