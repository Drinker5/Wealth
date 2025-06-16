using Microsoft.EntityFrameworkCore;
using Wealth.CurrencyManagement.Domain.Currency;
using Wealth.CurrencyManagement.Domain.ExchangeRate;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWork;

public class WealthDbContext : DbContext
{
    public DbSet<Currency> Currencies { get; internal init; }
    public DbSet<ExchangeRate> ExchangeRates { get; internal init; }
    private bool _commited;

    public WealthDbContext(DbContextOptions options)
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
        if (_commited)
            throw new Exception("can not commit twice within a scope in DbContext");

        _commited = true;
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        if (_commited)
            throw new Exception("can not commit twice within a scope in DbContext");

        _commited = true;
        return base.SaveChanges();
    }
}