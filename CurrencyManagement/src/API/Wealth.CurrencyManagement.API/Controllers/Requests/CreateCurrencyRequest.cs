namespace Wealth.CurrencyManagement.API.Controllers.Requests;

public class CreateCurrencyRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
}