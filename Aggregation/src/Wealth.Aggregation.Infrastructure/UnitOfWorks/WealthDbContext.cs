using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;

namespace Wealth.Aggregation.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project .\Aggregation\src\Wealth.Aggregation.Infrastructure --startup-project .\Aggregation\src\Wealth.Aggregation.API Name
/// dotnet ef database update --project .\Aggregation\src\Wealth.Aggregation.Infrastructure --startup-project .\Aggregation\src\Wealth.Aggregation.API
/// </summary>
public class WealthDbContext : WealthDbContextBase
{
    public virtual DbSet<StockAggregation> StockAggregations { get; internal init; }

    public WealthDbContext(DbContextOptions<WealthDbContext> options)
        : base(options)
    {
    }

    public override WealthDbContextBase CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=postgres;Database=Design");
        optionsBuilder.EnableSensitiveDataLogging();
        return new WealthDbContext(optionsBuilder.Options);
    }
}