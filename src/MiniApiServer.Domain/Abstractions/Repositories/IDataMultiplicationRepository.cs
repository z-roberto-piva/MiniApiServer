using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

/// <summary>
/// Persists results produced by multiplication operations.
/// </summary>
public interface IDataMultiplicationRepository
{
    /// <summary>
    /// Stores a multiplication result.
    /// </summary>
    Task AddAsync(DataMultiplication dataMultiplication, CancellationToken cancellationToken = default);
}
