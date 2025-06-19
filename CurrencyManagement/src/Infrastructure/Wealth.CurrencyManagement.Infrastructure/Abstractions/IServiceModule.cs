using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface IServiceModule
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}