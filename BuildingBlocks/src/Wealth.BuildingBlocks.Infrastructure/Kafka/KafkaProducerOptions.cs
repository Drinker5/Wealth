namespace Wealth.BuildingBlocks.Infrastructure.Kafka;

public class KafkaProducerOptions
{
    public const string Section = "KafkaProducer";

    public string BootstrapServers { get; set; } = "localhost:9092";
}