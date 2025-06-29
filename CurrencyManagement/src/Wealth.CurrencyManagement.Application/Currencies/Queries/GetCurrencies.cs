using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.Currencies.Queries;

public record GetCurrenciesQuery : IQuery<IEnumerable<CurrencyDTO>>;
