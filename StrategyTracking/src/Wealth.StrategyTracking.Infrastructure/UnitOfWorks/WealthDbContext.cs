using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;
using Wealth.BuildingBlocks.Infrastructure.EFCore.EntityConfigurations;
using Wealth.StrategyTracking.Domain.Strategies;
using Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project src\Wealth.StrategyTracking.Infrastructure --startup-project .\src\Wealth.StrategyTracking.API Name
/// dotnet ef database update --project src\Wealth.StrategyTracking.Infrastructure --startup-project .\src\Wealth.StrategyTracking.API
/// </summary>
public class WealthDbContext : DbContext, IDesignTimeDbContextFactory<WealthDbContext>, IUnitOfWork
{
    public virtual DbSet<Strategy> Strategies { get; internal init; }
    public virtual DbSet<OutboxMessage> OutboxMessages { get; internal init; }

    private IDbContextTransaction? transaction;

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
            modelBuilder.Entity<Strategy>().Property(i => i.Id)
                .HasValueGenerator<StrategyIdInMemoryValueGenerator>();
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
        configurationBuilder.Properties<InstrumentId>().HaveConversion<InstrumentIdConverter>();
        configurationBuilder.Properties<StrategyId>().HaveConversion<StrategyIdConverter>();
    }

    public async Task<IDisposable> BeginTransaction()
    {
        if (transaction != null)
            return transaction;

        transaction = await Database.BeginTransactionAsync();
        return transaction;
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        var result = await SaveChangesAsync(cancellationToken);
        if (transaction != null)
            await transaction.CommitAsync(cancellationToken);

        return result;
    }
}