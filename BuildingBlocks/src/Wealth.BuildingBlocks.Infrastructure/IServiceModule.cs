using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wealth.BuildingBlocks.Infrastructure;

public interface IServiceModule
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}