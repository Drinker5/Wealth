using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application.CommandScheduler;

namespace Wealth.BuildingBlocks.Infrastructure.CommandScheduler;

public class CommandSchedulerModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICommandsScheduler, CommandsScheduler>();
    }
}