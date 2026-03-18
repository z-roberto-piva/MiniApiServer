using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Configurations;

public sealed class DataSummConfiguration : IEntityTypeConfiguration<DataSumm>
{
    public void Configure(EntityTypeBuilder<DataSumm> builder)
    {
        builder.ToTable("data_summs");

        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id)
            .HasColumnName("id");

        builder.Property(entity => entity.DataInId)
            .HasColumnName("data_in_id")
            .IsRequired();

        builder.Property(entity => entity.Description)
            .HasColumnName("description")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(entity => entity.Result)
            .HasColumnName("result")
            .IsRequired();

        builder.Property<DateTime>("created_at_utc")
            .HasColumnName("created_at_utc")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}
