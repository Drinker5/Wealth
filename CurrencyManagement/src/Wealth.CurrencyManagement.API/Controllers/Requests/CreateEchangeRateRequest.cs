using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.API.Controllers.Requests;

public class CreateEchangeRateRequest
{
    public CurrencyCode From { get; set; }
    public CurrencyCode To { get; set; }
    public decimal Rate { get; set; }
    public DateOnly Date { get; set; }
}