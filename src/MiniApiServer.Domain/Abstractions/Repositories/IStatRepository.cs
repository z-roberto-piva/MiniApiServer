using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

/// <summary>
/// Persists daily statistics snapshots.
/// </summary>
public interface IStatRepository
{
    /// <summary>
    /// Stores a statistics row.
    /// </summary>
    Task AddAsync(Stat stat, CancellationToken cancellationToken = default);
}
