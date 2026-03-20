using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

/// <summary>
/// Persists results produced by division operations.
/// </summary>
public interface IDataDivisionRepository
{
    /// <summary>
    /// Stores a division result.
    /// </summary>
    Task AddAsync(DataDivision dataDivision, CancellationToken cancellationToken = default);
}
