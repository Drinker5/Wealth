using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Extensions;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

internal static class ConverterExtensions
{
    public static Money ToMoney(this MoneyValue moneyValue)
    {
        return new(CurrencyCodeExtensions.FromString(moneyValue.Currency), moneyValue);
    }
}