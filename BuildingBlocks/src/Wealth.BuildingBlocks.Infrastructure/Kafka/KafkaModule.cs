using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wealth.BuildingBlocks.Infrastructure.Kafka;

internal class KafkaModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IKafkaProducer, KafkaProducer>();
        services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
    }
}