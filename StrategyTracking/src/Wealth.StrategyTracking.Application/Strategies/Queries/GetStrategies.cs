using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Queries;

public record struct GetStrategies : IQuery<IEnumerable<StrategyDTO>>;

public class GetStrategiesQueryHandler(IStrategyRepository repository) : IQueryHandler<GetStrategies, IEnumerable<StrategyDTO>>
{
    public async Task<IEnumerable<StrategyDTO>> Handle(GetStrategies request, CancellationToken cancellationToken)
    {
        var strategies = await repository.GetStrategies();
        return strategies.Select(StrategyDTO.Convert);
    }
}