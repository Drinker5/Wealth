using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;

internal class KafkaModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IKafkaConsumer, KafkaConsumer>();

        services.AddOptions<KafkaConsumerOptions>()
            .BindConfiguration(KafkaConsumerOptions.Section)
            .ValidateOnStart()
            .ValidateDataAnnotations();
    }
}