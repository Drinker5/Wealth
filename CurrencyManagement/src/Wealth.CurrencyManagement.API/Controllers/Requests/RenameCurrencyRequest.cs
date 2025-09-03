using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.API.Controllers.Requests;

public class RenameCurrencyRequest
{
    public CurrencyId Id { get; set; }
    public string NewName { get; set; }
}