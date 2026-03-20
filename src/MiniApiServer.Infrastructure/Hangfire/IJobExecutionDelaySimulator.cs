namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Applies the optional artificial delay configured for Hangfire jobs.
/// </summary>
public interface IJobExecutionDelaySimulator
{
    /// <summary>
    /// Waits before executing a job when delay simulation is enabled.
    /// </summary>
    Task DelayAsync(string jobName, Guid dataInId, CancellationToken cancellationToken = default);
}
