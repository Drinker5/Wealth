using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.API.Controllers.Requests;

public class CreateCurrencyRequest
{
    public CurrencyId Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
}