using System.Data;
using MediatR;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection connection;

    public UnitOfWork(WealthDbContext context)
    {
        connection = context.CreateConnection();
    }

    public async Task<TResponse> Transaction<TResponse>(RequestHandlerDelegate<TResponse> next, CancellationToken token)
    {
        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var transaction = connection.BeginTransaction();
        try
        {
            var result = await next(token);
            transaction.Commit();
            return result;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}