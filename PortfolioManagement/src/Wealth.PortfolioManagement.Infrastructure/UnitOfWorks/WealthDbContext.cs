using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;
using Wealth.BuildingBlocks.Infrastructure.EFCore.EntityConfigurations;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project src\Wealth.PortfolioManagement.Infrastructure --startup-project .\src\Wealth.PortfolioManagement.API Name
/// dotnet ef database update --project src\Wealth.PortfolioManagement.Infrastructure --startup-project .\src\Wealth.PortfolioManagement.API
/// </summary>
public class WealthDbContext : DbContext, IDesignTimeDbContextFactory<WealthDbContext>
{
    public virtual DbSet<Portfolio> Portfolios { get; internal init; }
    public virtual DbSet<OutboxMessage> OutboxMessages { get; internal init; }

    public virtual DbSet<InstrumentOperation> InstrumentOperations { get; internal init; }
    public virtual DbSet<CurrencyOperation> CurrencyOperations { get; internal init; }

    public WealthDbContext()
    {
    }

    public WealthDbContext(DbContextOptions<WealthDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        if (Database.IsInMemory())
        {
            modelBuilder.Entity<Portfolio>().Property(i => i.Id)
                .HasValueGenerator<PortfolioIdInMemoryValueGenerator>();
        }

        base.OnModelCreating(modelBuilder);
    }

    public WealthDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=postgres;Database=Design");
        return new WealthDbContext(optionsBuilder.Options);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<CurrencyId>().HaveConversion<CurrencyIdConverter>();
        configurationBuilder.Properties<InstrumentId>().HaveConversion<InstrumentIdConverter>();
    }
}