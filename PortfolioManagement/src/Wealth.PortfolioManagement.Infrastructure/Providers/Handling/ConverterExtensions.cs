using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

internal static class ConverterExtensions
{
    public static Money ToMoney(this MoneyValue moneyValue) => new(moneyValue.Currency, moneyValue);
}