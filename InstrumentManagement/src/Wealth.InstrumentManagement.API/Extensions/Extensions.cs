using Wealth.InstrumentManagement.Domain.Repositories;
using Wealth.InstrumentManagement.Infrastructure.Repositories;

namespace Wealth.InstrumentManagement.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IInstrumentsRepository, InMemoryInstrumentRepository>();
        builder.Services.AddSingleton<IBondsRepository, InMemoryInstrumentRepository>();
        builder.Services.AddSingleton<IStocksRepository, InMemoryInstrumentRepository>();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });
    }
}
