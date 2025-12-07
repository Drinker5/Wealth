using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Queries;

public record struct GetStrategy(StrategyId StrategyId) : IQuery<StrategyDTO?>;

public class GetStrategyQueryHandler(IStrategyRepository repository) : IQueryHandler<GetStrategy, StrategyDTO?>
{
    public async Task<StrategyDTO?> Handle(GetStrategy request, CancellationToken cancellationToken)
    {
        var strategy = await repository.GetStrategy(request.StrategyId);
        return strategy == null ? null : StrategyDTO.Convert(strategy);
    }
}