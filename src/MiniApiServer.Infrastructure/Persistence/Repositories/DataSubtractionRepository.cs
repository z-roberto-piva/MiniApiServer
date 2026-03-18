using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

public sealed class DataSubtractionRepository(MiniApiServerDbContext dbContext) : IDataSubtractionRepository
{
    public async Task AddAsync(DataSubtraction dataSubtraction, CancellationToken cancellationToken = default)
    {
        await dbContext.DataSubtractions.AddAsync(dataSubtraction, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
