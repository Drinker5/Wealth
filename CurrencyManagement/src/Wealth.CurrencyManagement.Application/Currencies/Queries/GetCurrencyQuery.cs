using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.Currencies.Queries;

public record GetCurrencyQuery(CurrencyId Id) : IQuery<CurrencyDTO?>;