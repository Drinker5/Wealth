using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.API.Controllers.Requests;

public record CreateExchangeRateRequest(
    CurrencyCode From,
    CurrencyCode To,
    decimal Rate,
    DateOnly Date);