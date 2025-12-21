using System.ComponentModel.DataAnnotations;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public class TBankInstrumentsProviderOptions
{
    public const string Section = "InstrumentsProvider:TBank";

    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; }
}