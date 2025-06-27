using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wealth.CurrencyManagement.Infrastructure.Abstractions;

public interface IServiceModule
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}