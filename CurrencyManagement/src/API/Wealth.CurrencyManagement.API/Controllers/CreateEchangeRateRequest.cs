namespace Wealth.CurrencyManagement.API.Controllers;

public class CreateEchangeRateRequest
{
    public string FromId { get; set; }
    public string ToId { get; set; }
    public decimal Rate { get; set; }
    public DateTime Date { get; set; }
}