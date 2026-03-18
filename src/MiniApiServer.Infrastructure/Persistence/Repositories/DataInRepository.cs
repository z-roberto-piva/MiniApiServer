using Microsoft.EntityFrameworkCore;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

public sealed class DataInRepository(MiniApiServerDbContext dbContext) : IDataInRepository
{
    public async Task AddAsync(DataIn dataIn, CancellationToken cancellationToken = default)
    {
        await dbContext.DataIns.AddAsync(dataIn, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<DataIn?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.DataIns.SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);

    public async Task UpdateAsync(DataIn dataIn, CancellationToken cancellationToken = default)
    {
        dbContext.DataIns.Update(dataIn);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
