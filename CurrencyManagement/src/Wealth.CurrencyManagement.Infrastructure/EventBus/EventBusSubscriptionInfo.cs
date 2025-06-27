namespace Wealth.CurrencyManagement.Infrastructure.EventBus;

public class EventBusSubscriptionInfo
{
    public Dictionary<string, Type> EventTypes { get; } = [];
}
