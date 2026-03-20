using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IStatRepository"/>.
/// </summary>
public sealed class StatRepository(MiniApiServerDbContext dbContext) : IStatRepository
{
    /// <inheritdoc />
    public async Task AddAsync(Stat stat, CancellationToken cancellationToken = default)
    {
        await dbContext.Stats.AddAsync(stat, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
