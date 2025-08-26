namespace Wealth.InstrumentManagement.Infrastructure.DbSeeding;

public interface IDbSeeder
{
    Task SeedAsync(CancellationToken token = default);
}