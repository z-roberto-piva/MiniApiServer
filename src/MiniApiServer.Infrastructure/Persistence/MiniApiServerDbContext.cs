using Microsoft.EntityFrameworkCore;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence;

public sealed class MiniApiServerDbContext(DbContextOptions<MiniApiServerDbContext> options) : DbContext(options)
{
    public const string Schema = "mini_api_server";

    public DbSet<DataIn> DataIns => Set<DataIn>();

    public DbSet<DataSumm> DataSumms => Set<DataSumm>();

    public DbSet<DataSubtraction> DataSubtractions => Set<DataSubtraction>();

    public DbSet<DataMultiplication> DataMultiplications => Set<DataMultiplication>();

    public DbSet<DataDivision> DataDivisions => Set<DataDivision>();

    public DbSet<Stat> Stats => Set<Stat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniApiServerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
