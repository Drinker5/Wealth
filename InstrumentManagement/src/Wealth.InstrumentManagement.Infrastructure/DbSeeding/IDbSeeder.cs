using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.DbSeeding;

public interface IDbSeeder
{
    Task SeedAsync(WealthDbContext context);
}