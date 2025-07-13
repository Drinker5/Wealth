using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Money = Wealth.CurrencyManagement.Domain.ExchangeRates.Money;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Queries;

public record ExchangeQuery(
    Money Money,
    CurrencyId TargetCurrencyId,
    DateOnly Date) : IQuery<Money?>;