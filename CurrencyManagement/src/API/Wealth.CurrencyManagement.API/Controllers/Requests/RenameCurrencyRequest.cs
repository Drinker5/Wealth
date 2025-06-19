namespace Wealth.CurrencyManagement.API.Controllers.Requests;

public class RenameCurrencyRequest
{
    public string Id { get; set; }
    public string NewName { get; set; }
}