using MiniApiServer.Application.Contracts;

namespace MiniApiServer.Application.Abstractions.Queries;

public interface IDailyOperationsSummaryReader
{
    Task<DailyOperationsSummary> GetForDateAsync(DateOnly date, CancellationToken cancellationToken = default);
}
