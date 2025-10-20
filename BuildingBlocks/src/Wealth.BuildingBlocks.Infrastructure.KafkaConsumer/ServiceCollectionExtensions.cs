using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTopicHandler<T, H>(
        this IServiceCollection services,
        IConfiguration configuration,
        string section)
        where T : Google.Protobuf.IMessage
        where H : IMessageHandler<T>
    {
        services.TryAddSingleton(typeof(H));
        return services.AddHostedService(sp =>
        {
            var topicOptions = new TopicOptions();
            configuration.GetRequiredSection(section).Bind(topicOptions);
            return new ConsumerHostedService<T>(
                sp.GetRequiredService<IKafkaConsumer>(),
                topicOptions,
                sp.GetRequiredService<H>(),
                sp.GetRequiredService<ILogger<ConsumerHostedService<T>>>());
        });
    }
}