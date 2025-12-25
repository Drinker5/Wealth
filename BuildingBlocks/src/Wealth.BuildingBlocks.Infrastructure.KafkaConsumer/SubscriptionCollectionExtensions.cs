using Confluent.Kafka;
using Eventso.Subscription;
using Eventso.Subscription.Configurations;
using Eventso.Subscription.Hosting;
using Eventso.Subscription.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;

public static class SubscriptionCollectionExtensions
{
    public static void AddBatch(this ISubscriptionCollection subs, IServiceProvider sp, string section, IMessageDeserializer deserializer)
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        var kafkaConsumerOptions = sp.GetRequiredService<IOptions<KafkaConsumerOptions>>().Value;
        var topicOptions = new TopicOptions();
        configuration.GetRequiredSection(section).Bind(topicOptions);
        var consumerSettings = new ConsumerSettings(
            kafkaConsumerOptions.BootstrapServers,
            kafkaConsumerOptions.GroupId,
            autoOffsetReset: AutoOffsetReset.Latest)
        {
            Topic = topicOptions.Topic
        };
        subs.AddBatch(
            consumerSettings,
            new BatchConfiguration
            {
                MaxBatchSize = topicOptions.MaxBatchSize,
                BatchTriggerTimeout = topicOptions.BatchTriggerTimeout,
                MaxBufferSize = 0
            },
            deserializer);
    }
}