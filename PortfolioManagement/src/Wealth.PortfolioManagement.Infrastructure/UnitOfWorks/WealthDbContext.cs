using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.Repositories;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project src\Wealth.PortfolioManagement.Infrastructure --startup-project .\src\Wealth.PortfolioManagement.API Name
/// dotnet ef database update --project src\Wealth.PortfolioManagement.Infrastructure --startup-project .\src\Wealth.PortfolioManagement.API
/// </summary>
public class WealthDbContext : DbContext, IDesignTimeDbContextFactory<WealthDbContext>
{
    public virtual DbSet<Portfolio> Portfolios { get; internal init; }
    public virtual DbSet<OutboxMessage> OutboxMessages { get; internal init; }

    private bool commited;

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

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (commited)
            throw new Exception("can not commit twice within a scope in DbContext");

        commited = true;
        return base.SaveChangesAsync(cancellationToken);
    }

    public WealthDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=postgres;Database=Design");
        return new WealthDbContext(optionsBuilder.Options);
    }
}