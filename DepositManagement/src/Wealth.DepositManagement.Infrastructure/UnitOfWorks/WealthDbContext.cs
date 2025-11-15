using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore;
using Wealth.DepositManagement.Domain.DepositOperations;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.DepositManagement.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add Init --project .\DepositManagement\src\Wealth.DepositManagement.Infrastructure\ --startup-project .\DepositManagement\src\Wealth.DepositManagement.API\
/// </summary>
public class WealthDbContext : WealthDbContextBase
{
    public virtual DbSet<Deposit> Deposits { get; internal init; }
    public virtual DbSet<DepositOperation> DepositOperations { get; internal init; }

    public WealthDbContext(DbContextOptions<WealthDbContext> options)
        : base(options)
    {
    }

    protected override void OnInMemoryModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deposit>().Property(i => i.Id)
            .HasValueGenerator<DepositIdInMemoryValueGenerator>();

        modelBuilder.Entity<Deposit>().Ignore(i => i.Investment);
    }

    protected override void AdditionalConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DepositId>().HaveConversion<DepositIdConverter>();
    }

    public override WealthDbContextBase CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=postgres;Database=Design");
        return new WealthDbContext(optionsBuilder.Options);
    }
}