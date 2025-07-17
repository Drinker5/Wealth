using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.EntityConfigurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Key).IsRequired();
        builder.Property(x => x.Data).IsRequired();
        builder.Property(x => x.Type).HasMaxLength(255).IsRequired();
        builder.Property(i => i.OccurredOn).IsRequired();
        builder.Property(i => i.ProcessedOn);
        builder.HasNoDiscriminator();
    }
}