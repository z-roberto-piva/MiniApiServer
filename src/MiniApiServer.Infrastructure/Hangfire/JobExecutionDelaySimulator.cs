using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Applies a configured random delay before background job execution to simulate long-running work.
/// </summary>
public sealed class JobExecutionDelaySimulator(
    IOptions<JobExecutionDelayOptions> options,
    IRandomDelaySelector randomDelaySelector,
    IJobDelayAwaiter jobDelayAwaiter,
    ILogger<JobExecutionDelaySimulator> logger) : IJobExecutionDelaySimulator
{
    /// <inheritdoc />
    public async Task DelayAsync(string jobName, Guid dataInId, CancellationToken cancellationToken = default)
    {
        var configuredDurations = options.Value.AllowedDurationsInSeconds;
        var allowedDurations = configuredDurations is { Length: > 0 }
            ? configuredDurations.Intersect(JobExecutionDelayOptions.DefaultAllowedDurationsInSeconds).Distinct().Order().ToArray()
            : JobExecutionDelayOptions.DefaultAllowedDurationsInSeconds.ToArray();

        if (!options.Value.Enabled || allowedDurations.Length == 0)
        {
            return;
        }

        var selectedDurationInSeconds = allowedDurations[randomDelaySelector.NextIndex(allowedDurations.Length)];
        var selectedDelay = TimeSpan.FromSeconds(selectedDurationInSeconds);

        logger.LogInformation(
            "Applying artificial delay of {DelayInSeconds} seconds to job {JobName} for data input {DataInId}.",
            selectedDurationInSeconds,
            jobName,
            dataInId);

        await jobDelayAwaiter.DelayAsync(selectedDelay, cancellationToken);

        logger.LogInformation(
            "Artificial delay completed for job {JobName} on data input {DataInId}.",
            jobName,
            dataInId);
    }
}
