namespace Wealth.CurrencyManagement.API.Controllers;

public class CreateCurrencyRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
}