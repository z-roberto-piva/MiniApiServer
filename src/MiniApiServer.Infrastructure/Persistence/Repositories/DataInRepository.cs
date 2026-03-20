using Microsoft.EntityFrameworkCore;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IDataInRepository"/>.
/// </summary>
public sealed class DataInRepository(MiniApiServerDbContext dbContext) : IDataInRepository
{
    /// <inheritdoc />
    public async Task AddAsync(DataIn dataIn, CancellationToken cancellationToken = default)
    {
        await dbContext.DataIns.AddAsync(dataIn, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<DataIn?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.DataIns.SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task UpdateAsync(DataIn dataIn, CancellationToken cancellationToken = default)
    {
        dbContext.DataIns.Update(dataIn);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
