namespace Wealth.InstrumentManagement.Application.Options;

public sealed class PriceUpdaterOptions
{
    public const string Section = "PriceUpdater";

    public TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(1);

    public TimeSpan OlderThan { get; set; } = TimeSpan.FromHours(1);
}