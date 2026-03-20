using MiniApiServer.Application.Abstractions.Queries;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.UseCases.GenerateDailyStats;

/// <summary>
/// Builds and persists the daily statistics snapshot from aggregated operation data.
/// </summary>
public sealed class GenerateDailyStatsUseCase(IDailyOperationsSummaryReader dailyOperationsSummaryReader, IStatRepository statRepository)
{
    /// <summary>
    /// Generates the statistics snapshot for the requested day.
    /// </summary>
    public async Task<GenerateDailyStatsResult> ExecuteAsync(GenerateDailyStatsCommand command, CancellationToken cancellationToken = default)
    {
        var summary = await dailyOperationsSummaryReader.GetForDateAsync(command.Date, cancellationToken);
        var stat = Stat.Create(
            summary.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            summary.NumberOfOperations,
            summary.TotalOfSums,
            summary.TotalOfSubtractions,
            summary.TotalOfMultiplications,
            summary.TotalOfDivisions);

        await statRepository.AddAsync(stat, cancellationToken);

        return new GenerateDailyStatsResult(
            command.Date,
            stat.NumberOfOperations,
            stat.TotalOfSums,
            stat.TotalOfSubtractions,
            stat.TotalOfMultiplications,
            stat.TotalOfDivisions);
    }
}
