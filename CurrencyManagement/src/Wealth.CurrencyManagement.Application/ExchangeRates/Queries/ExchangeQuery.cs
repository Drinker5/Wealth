using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Queries;

public record ExchangeQuery(
    Money Money,
    CurrencyCode TargetCurrency,
    DateOnly Date) : IQuery<Money?>;