using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.GenerateDailyStats;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class GenerateDailyStatsRecurringJob(GenerateDailyStatsUseCase useCase)
{
    public Task ExecuteAsync()
    {
        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        return useCase.ExecuteAsync(new GenerateDailyStatsCommand(todayUtc));
    }
}
