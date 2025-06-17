using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wealth.CurrencyManagement.Infrastructure.Interfaces;

public interface IServiceModule
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}