namespace Wealth.CurrencyManagement.Application.Abstractions;

public class DeferredOperationPollingOptions
{
    public const string Section = "DeferredOperationPolling";

    public bool Enabled { get; set; } = false;
    public int RetryCount { get; set; } = 10;
}