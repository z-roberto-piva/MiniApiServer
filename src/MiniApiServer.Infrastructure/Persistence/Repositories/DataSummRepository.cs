using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

public sealed class DataSummRepository(MiniApiServerDbContext dbContext) : IDataSummRepository
{
    public async Task AddAsync(DataSumm dataSumm, CancellationToken cancellationToken = default)
    {
        await dbContext.DataSumms.AddAsync(dataSumm, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
