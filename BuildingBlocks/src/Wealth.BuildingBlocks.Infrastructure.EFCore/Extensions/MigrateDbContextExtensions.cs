using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;

public static class MigrateDbContextExtensions
{
    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services)
        where TContext : DbContext
        => services.AddMigration<TContext>((_, _) => Task.CompletedTask);

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services, Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        return services.AddHostedService(sp => new MigrationHostedService<TContext>(sp, seeder));
    }

    public static IServiceCollection AddMigration<TContext, TDbSeeder>(this IServiceCollection services)
        where TContext : DbContext
        where TDbSeeder : class, IDbSeeder<TContext>
    {
        return services
            .AddScoped<TDbSeeder>()
            .AddMigration<TContext>((context, sp) => sp.GetRequiredService<TDbSeeder>().SeedAsync(context));
    }

    private static async Task MigrateDbContextAsync<TContext>(this IServiceProvider services, Func<TContext, IServiceProvider, Task> seeder) where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var scopeServices = scope.ServiceProvider;
        var logger = scopeServices.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            var context = scopeServices.GetRequiredService<TContext>();
            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(() => InvokeSeeder(seeder, context, scopeServices));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            throw;
        }
    }

    private static async Task InvokeSeeder<TContext>(Func<TContext, IServiceProvider, Task> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        await context.Database.MigrateAsync();
        await seeder(context, services);
    }

    private class MigrationHostedService<TContext>(IServiceProvider serviceProvider, Func<TContext, IServiceProvider, Task> seeder)
        : IHostedService where TContext : DbContext
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return serviceProvider.MigrateDbContextAsync(seeder);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}