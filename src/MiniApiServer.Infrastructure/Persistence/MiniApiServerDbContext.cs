using Microsoft.EntityFrameworkCore;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence;

public sealed class MiniApiServerDbContext(DbContextOptions<MiniApiServerDbContext> options) : DbContext(options)
{
    public DbSet<DataIn> DataIns => Set<DataIn>();

    public DbSet<DataSumm> DataSumms => Set<DataSumm>();

    public DbSet<DataSubtraction> DataSubtractions => Set<DataSubtraction>();

    public DbSet<Stat> Stats => Set<Stat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniApiServerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
