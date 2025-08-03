using MediatR;
using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore;

public class EFCoreUnitOfWork(DbContext context) : IUnitOfWork
{
    public Task<TResponse> Transaction<TResponse>(RequestHandlerDelegate<TResponse> next, CancellationToken token)
    {
        var executionStrategy = context.Database.CreateExecutionStrategy();
        return executionStrategy.ExecuteAsync(
            state: next,
            operation: Operation,
            verifySucceeded: null,
            cancellationToken: token);
    }
    
    private static async Task<TResponse> Operation<TResponse>(DbContext context, RequestHandlerDelegate<TResponse> next, CancellationToken t)
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
    }
}