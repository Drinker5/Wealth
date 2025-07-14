namespace Wealth.Aggregation.Infrastructure.EventBus;

public class KafkaConsumerOptions
{
    public const string Section = "KafkaConsumer";
    
    public string BootstrapServers { get; set; } = "localhost:9092";
    public string GroupId { get; set; } = "aggregation";
    public IEnumerable<string> Topics { get; set; } = [
        "currency-management",
        "instrument-management",
        "portfolio-management",
    ];

    public string EventNameHeader { get; set; } = "EventName";
}