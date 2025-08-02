using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;
using Wealth.StrategyTracking.Infrastructure.UnitOfWorks;

namespace Wealth.StrategyTracking.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;

public class CommandUnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IUnitOfWork unitOfWork;
    private readonly WealthDbContext context;

    public CommandUnitOfWorkBehavior(IUnitOfWork unitOfWork, WealthDbContext context)
    {
        this.unitOfWork = unitOfWork;
        this.context = context;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
    {
        var executionStrategy = context.Database.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(
            state: next,
            operation: async (context, next, t) =>
            {
                await using var transaction = await context.Database.BeginTransactionAsync(t);
                try
                {
                    var result = await next(t);
                    await context.SaveChangesAsync(t);
                    await transaction.CommitAsync(t);
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(t);
                    throw;
                }
            },
            verifySucceeded: null,
            cancellationToken: token);
    }
}