using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Currencies.Queries;

public record GetCurrencyQuery(CurrencyId Id) : IQuery<CurrencyDTO?>;