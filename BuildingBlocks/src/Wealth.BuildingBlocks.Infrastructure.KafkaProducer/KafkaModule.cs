using Confluent.Kafka;
using Eventso.KafkaProducer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaProducer;

internal class KafkaModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<KafkaProducerOptions>()
            .BindConfiguration(KafkaProducerOptions.Section)
            .ValidateOnStart()
            .ValidateDataAnnotations();

        services.AddSingleton<IProducer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<KafkaProducerOptions>>();
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
            };

            return new ProducerBuilder<byte[], byte[]>(producerConfig).BuildBinary();
        });
    }
}