using MiniApiServer.Application.Contracts;

namespace MiniApiServer.Application.Abstractions.Queries;

/// <summary>
/// Reads the aggregated operation totals required to build daily statistics.
/// </summary>
public interface IDailyOperationsSummaryReader
{
    /// <summary>
    /// Returns the aggregated totals for the specified day.
    /// </summary>
    Task<DailyOperationsSummary> GetForDateAsync(DateOnly date, CancellationToken cancellationToken = default);
}
