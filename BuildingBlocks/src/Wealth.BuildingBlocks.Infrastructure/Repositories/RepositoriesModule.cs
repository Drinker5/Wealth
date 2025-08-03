using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application.CommandScheduler;

namespace Wealth.BuildingBlocks.Infrastructure.Repositories;

internal class RepositoriesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDeferredOperationRepository, InMemoryDefferedOperationRepository>();
    }
}