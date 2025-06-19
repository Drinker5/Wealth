namespace Wealth.CurrencyManagement.API.Controllers.Requests;

public class CreateEchangeRateRequest
{
    public string FromId { get; set; }
    public string ToId { get; set; }
    public decimal Rate { get; set; }
    public DateOnly Date { get; set; }
}