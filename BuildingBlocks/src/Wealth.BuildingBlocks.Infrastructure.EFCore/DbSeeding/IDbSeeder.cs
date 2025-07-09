using Microsoft.EntityFrameworkCore;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}