namespace Wealth.BuildingBlocks.Infrastructure.KafkaProducer;

public class KafkaProducerOptions
{
    public const string Section = "KafkaProducer";

    public string BootstrapServers { get; set; } = "localhost:9092";
}