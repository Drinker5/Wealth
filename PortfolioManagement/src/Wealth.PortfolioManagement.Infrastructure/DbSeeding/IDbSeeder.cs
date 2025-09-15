using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.DbSeeding;

public interface IDbSeeder
{
    Task SeedAsync(WealthDbContext context);
}