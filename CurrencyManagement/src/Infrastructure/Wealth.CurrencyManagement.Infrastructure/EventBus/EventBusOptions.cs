namespace Wealth.CurrencyManagement.Infrastructure.EventBus;

public class EventBusOptions
{
    public const string Section = "EventBus";
    
    public string SubscriptionClientName { get; set; }
    public int RetryCount { get; set; } = 10;
}
