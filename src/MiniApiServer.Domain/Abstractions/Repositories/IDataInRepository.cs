using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

public interface IDataInRepository
{
    Task AddAsync(DataIn dataIn, CancellationToken cancellationToken = default);

    Task<DataIn?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(DataIn dataIn, CancellationToken cancellationToken = default);
}
