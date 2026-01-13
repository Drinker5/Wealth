using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpJuice.Essentials;

namespace Wealth.BuildingBlocks.Infrastructure;

public class ApplicationModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IClock, Clock>();
    }
}