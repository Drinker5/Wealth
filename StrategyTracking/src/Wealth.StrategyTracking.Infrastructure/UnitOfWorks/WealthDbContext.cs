using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure.EFCore;
using Wealth.BuildingBlocks.Infrastructure.EFCore.EntityConfigurations;
using Wealth.StrategyTracking.Domain.Strategies;
using Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project src\Wealth.StrategyTracking.Infrastructure --startup-project .\src\Wealth.StrategyTracking.API Name
/// dotnet ef database update --project src\Wealth.StrategyTracking.Infrastructure --startup-project .\src\Wealth.StrategyTracking.API
/// </summary>
public class WealthDbContext : WealthDbContextBase
{
    public virtual DbSet<Strategy> Strategies { get; internal init; }

    public WealthDbContext()
    {
    }

    public WealthDbContext(DbContextOptions<WealthDbContext> options)
        : base(options)
    {
    }

    protected override void OnInMemoryModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Strategy>().Property(i => i.Id)
            .HasValueGenerator<StrategyIdInMemoryValueGenerator>();
    }

    public override WealthDbContextBase CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=postgres;Database=Design");
        return new WealthDbContext(optionsBuilder.Options);
    }

    protected override void AdditionalConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<StrategyId>().HaveConversion<StrategyIdConverter>();
    }
}