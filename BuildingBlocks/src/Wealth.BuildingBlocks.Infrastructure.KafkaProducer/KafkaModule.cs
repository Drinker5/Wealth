using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaProducer;

internal class KafkaModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IKafkaProducer, KafkaProducer>();

        services.AddOptions<KafkaProducerOptions>()
            .BindConfiguration(KafkaProducerOptions.Section)
            .ValidateOnStart()
            .ValidateDataAnnotations();
    }
}