namespace Wealth.InstrumentManagement.Infrastructure.DbSeeding;

public interface IDbSeeder
{
    Task SeedAsync();
}