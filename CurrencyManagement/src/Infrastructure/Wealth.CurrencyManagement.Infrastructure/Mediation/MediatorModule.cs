using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Application.Currency.Commands;

namespace Wealth.CurrencyManagement.Infrastructure.Mediation;

public static class MediatorModule
{
    public static void RegisterMediatorModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCurrencyCommandHandler).Assembly));
    }
}
