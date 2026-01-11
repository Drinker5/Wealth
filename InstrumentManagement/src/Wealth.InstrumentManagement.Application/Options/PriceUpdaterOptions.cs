namespace Wealth.InstrumentManagement.Application.Options;

public sealed class PriceUpdaterOptions
{
    public const string Section = "PriceUpdater";
    
    public TimeSpan OlderThan { get; set; } = TimeSpan.FromHours(1);  
}