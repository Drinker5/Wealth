namespace Wealth.CurrencyManagement.Infrastructure.EventBus;

public class EventBusOptions
{
    public const string Section = "EventBus";
    
    public EventBusProvider Provider { get; set; } = EventBusProvider.RabbitMQ;
    public string SubscriptionClientName { get; set; }
    public int RetryCount { get; set; } = 10;

    public enum EventBusProvider
    {
        RabbitMQ,
    }
}
