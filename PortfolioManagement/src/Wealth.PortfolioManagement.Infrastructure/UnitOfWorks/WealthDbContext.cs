using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;
using Wealth.BuildingBlocks.Infrastructure.EFCore.EntityConfigurations;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;
using BondIdConverter = Wealth.BuildingBlocks.Infrastructure.EFCore.Converters.BondIdConverter;
using StockIdConverter = Wealth.BuildingBlocks.Infrastructure.EFCore.Converters.StockIdConverter;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project src\Wealth.PortfolioManagement.Infrastructure --startup-project .\src\Wealth.PortfolioManagement.API Name
/// dotnet ef database update --project src\Wealth.PortfolioManagement.Infrastructure --startup-project .\src\Wealth.PortfolioManagement.API
/// </summary>
public class WealthDbContext : WealthDbContextBase
{
    public virtual DbSet<Portfolio> Portfolios { get; internal init; }

    public virtual DbSet<Operation> InstrumentOperations { get; internal init; }
    public virtual DbSet<CurrencyOperation> CurrencyOperations { get; internal init; }

    public WealthDbContext(DbContextOptions<WealthDbContext> options)
        : base(options)
    {
    }

    protected override void OnInMemoryModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Portfolio>().Property(i => i.Id)
            .HasValueGenerator<PortfolioIdInMemoryValueGenerator>();
    }

    public override WealthDbContextBase CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=postgres;Database=Design");
        return new WealthDbContext(optionsBuilder.Options);
    }

    protected override void AdditionalConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<PortfolioId>().HaveConversion<PortfolioIdConverter>();
    }
}