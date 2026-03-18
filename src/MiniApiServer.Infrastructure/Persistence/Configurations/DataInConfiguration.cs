using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Configurations;

public sealed class DataInConfiguration : IEntityTypeConfiguration<DataIn>
{
    public void Configure(EntityTypeBuilder<DataIn> builder)
    {
        builder.ToTable("data_in");

        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id)
            .HasColumnName("id");

        builder.Property(entity => entity.Description)
            .HasColumnName("description")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(entity => entity.DataA)
            .HasColumnName("data_a")
            .IsRequired();

        builder.Property(entity => entity.DataB)
            .HasColumnName("data_b")
            .IsRequired();

        builder.Property(entity => entity.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();

        builder.Property<DateTime>("created_at_utc")
            .HasColumnName("created_at_utc")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}
