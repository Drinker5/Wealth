using System.Data;

namespace Wealth.BuildingBlocks.Infrastructure.Repositories;

public interface IConnectionFactory
{
    public IDbConnection CreateConnection();
    public IDbConnection CreateMasterConnection();
}