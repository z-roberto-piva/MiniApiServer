using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

/// <summary>
/// Persists and retrieves <see cref="DataIn"/> records.
/// </summary>
public interface IDataInRepository
{
    /// <summary>
    /// Persists a new input.
    /// </summary>
    Task AddAsync(DataIn dataIn, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the input with the specified identifier, if present.
    /// </summary>
    Task<DataIn?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists changes applied to an existing input.
    /// </summary>
    Task UpdateAsync(DataIn dataIn, CancellationToken cancellationToken = default);
}
