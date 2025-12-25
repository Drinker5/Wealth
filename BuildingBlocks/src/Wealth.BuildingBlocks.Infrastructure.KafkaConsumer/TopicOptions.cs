namespace Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;

public sealed class TopicOptions
{
    public string Topic { get; set; }
    public int MaxBatchSize { get; set; } = 1000;
    public TimeSpan BatchTriggerTimeout { get; set; } = TimeSpan.FromMinutes(1);
}