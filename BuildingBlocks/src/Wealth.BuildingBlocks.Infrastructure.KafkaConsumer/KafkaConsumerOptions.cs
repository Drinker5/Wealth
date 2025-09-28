using System.ComponentModel.DataAnnotations;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;

public class KafkaConsumerOptions
{
    public const string Section = "KafkaConsumer";

    public string BootstrapServers { get; set; } = "localhost:9092";
    
    [Required]
    public string GroupId { get; set; }
}