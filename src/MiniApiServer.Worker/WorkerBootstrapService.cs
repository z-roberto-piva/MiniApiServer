using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniApiServer.Application.Abstractions.Jobs;
using MiniApiServer.Infrastructure.Hangfire;

namespace MiniApiServer.Worker;

public sealed class WorkerBootstrapService(
    IRecurringJobManager recurringJobManager,
    IConfiguration configuration,
    IBackgroundJobCategoryResolver backgroundJobCategoryResolver,
    IBackgroundJobQueueResolver queueResolver,
    ILogger<WorkerBootstrapService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cronExpression = configuration["Hangfire:RecurringJobs:DailyStatsCron"] ?? "59 23 * * *";
        var recurringJobCategory = backgroundJobCategoryResolver.ResolveCategory(BackgroundJobType.GenerateDailyStats);
        var recurringJobQueue = queueResolver.ResolveQueue(recurringJobCategory);

        recurringJobManager.AddOrUpdate<GenerateDailyStatsRecurringJob>(
            recurringJobId: "generate-daily-stats",
            queue: recurringJobQueue,
            methodCall: job => job.ExecuteAsync(),
            cronExpression: cronExpression,
            options: new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            });

        logger.LogInformation(
            "MiniApiServer.Worker avviato. Recurring job 'generate-daily-stats' registrato con cron '{CronExpression}' sulla queue '{QueueName}'.",
            cronExpression,
            recurringJobQueue);
        return Task.CompletedTask;
    }
}
