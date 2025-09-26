namespace Wealth.BuildingBlocks.Infrastructure.Kafka;

public class KafkaConsumerOptions
{
    public const string Section = "KafkaConsumer";

    public string BootstrapServers { get; set; } = "localhost:9092";
    public string GroupId { get; set; }
}