using MiniApiServer.Application.Abstractions.Queries;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.UseCases.GenerateDailyStats;

public sealed class GenerateDailyStatsUseCase(IDailyOperationsSummaryReader dailyOperationsSummaryReader, IStatRepository statRepository)
{
    public async Task<GenerateDailyStatsResult> ExecuteAsync(GenerateDailyStatsCommand command, CancellationToken cancellationToken = default)
    {
        var summary = await dailyOperationsSummaryReader.GetForDateAsync(command.Date, cancellationToken);
        var stat = Stat.Create(
            summary.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            summary.NumberOfOperations,
            summary.TotalOfSums,
            summary.TotalOfSubtractions);

        await statRepository.AddAsync(stat, cancellationToken);

        return new GenerateDailyStatsResult(
            command.Date,
            stat.NumberOfOperations,
            stat.TotalOfSums,
            stat.TotalOfSubtractions);
    }
}
