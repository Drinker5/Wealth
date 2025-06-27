using Microsoft.EntityFrameworkCore;

namespace Wealth.CurrencyManagement.Infrastructure.DbSeeding;

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}