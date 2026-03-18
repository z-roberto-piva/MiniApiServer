using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

public sealed class StatRepository(MiniApiServerDbContext dbContext) : IStatRepository
{
    public async Task AddAsync(Stat stat, CancellationToken cancellationToken = default)
    {
        await dbContext.Stats.AddAsync(stat, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
