using System.ComponentModel.DataAnnotations;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public class TBankOperationProviderOptions
{
    public const string Section = "OperationProvider:TBank";

    [Required(ErrorMessage = "Account is required")]
    public string AccountId { get; set; }

    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; }

    public Dictionary<string, string> FixInstrumentUid { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}