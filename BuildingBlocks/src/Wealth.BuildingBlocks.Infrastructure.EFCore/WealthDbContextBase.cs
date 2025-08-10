using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;
using Wealth.BuildingBlocks.Infrastructure.EFCore.EntityConfigurations;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore;

/// <summary>
/// dotnet ef migrations add Init --project .\Infrastructure\ --startup-project .\API\
/// </summary>
public abstract class WealthDbContextBase : DbContext, IDesignTimeDbContextFactory<WealthDbContextBase>
{
    public virtual DbSet<OutboxMessage> OutboxMessages { get; internal init; }

    public WealthDbContextBase()
    {
    }

    public WealthDbContextBase(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        if (Database.IsInMemory())
            OnInMemoryModelCreating(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
    
    protected virtual void OnInMemoryModelCreating(ModelBuilder modelBuilder)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<CurrencyId>().HaveConversion<CurrencyIdConverter>();
        configurationBuilder.Properties<StockId>().HaveConversion<StockIdConverter>();
        configurationBuilder.Properties<BondId>().HaveConversion<BondIdConverter>();
    }

    public abstract WealthDbContextBase CreateDbContext(string[] args);
}