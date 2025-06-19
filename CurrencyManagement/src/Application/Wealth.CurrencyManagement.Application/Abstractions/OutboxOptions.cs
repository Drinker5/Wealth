namespace Wealth.CurrencyManagement.Application.Abstractions;

public class OutboxOptions
{
    public const string Section = "Outbox";
    public int RetryCount { get; set; } = 10;
}