using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniApiServer.Infrastructure.Hangfire;

namespace MiniApiServer.Worker;

public sealed class WorkerBootstrapService(
    IRecurringJobManager recurringJobManager,
    IConfiguration configuration,
    ILogger<WorkerBootstrapService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cronExpression = configuration["Hangfire:RecurringJobs:DailyStatsCron"] ?? "59 23 * * *";

        recurringJobManager.AddOrUpdate<GenerateDailyStatsRecurringJob>(
            recurringJobId: "generate-daily-stats",
            methodCall: job => job.ExecuteAsync(),
            cronExpression: cronExpression,
            options: new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            });

        logger.LogInformation("MiniApiServer.Worker avviato. Recurring job 'generate-daily-stats' registrato con cron '{CronExpression}'.", cronExpression);
        return Task.CompletedTask;
    }
}
