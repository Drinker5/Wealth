using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Infrastructure.Repositories;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Data).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.Type).HasMaxLength(255).IsRequired();
        builder.Property(i => i.OccurredOn).IsRequired();
        builder.Property(i => i.ProcessedOn);
        builder.HasNoDiscriminator();
    }
}