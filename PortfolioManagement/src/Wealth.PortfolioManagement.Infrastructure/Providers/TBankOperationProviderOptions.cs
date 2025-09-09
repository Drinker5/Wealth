using System.ComponentModel.DataAnnotations;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public class TBankOperationProviderOptions
{
    public const string Section = "OperationProvider:TBank";

    [Required(ErrorMessage = "Url is required")]
    public string Url { get; set; }
    
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; }
}