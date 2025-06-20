namespace Wealth.CurrencyManagement.Application.Abstractions;

public class OutboxPollingOptions
{
    public const string Section = "OutboxPolling";

    public bool Enabled { get; set; } = false;
    public int RetryCount { get; set; } = 10;
}