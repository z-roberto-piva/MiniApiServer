using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.GenerateDailyStats;

namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Hangfire wrapper that executes the daily statistics use case.
/// </summary>
public sealed class GenerateDailyStatsRecurringJob(GenerateDailyStatsUseCase useCase)
{
    /// <summary>
    /// Generates statistics for the current UTC day.
    /// </summary>
    public Task ExecuteAsync()
    {
        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        return useCase.ExecuteAsync(new GenerateDailyStatsCommand(todayUtc));
    }
}
