namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Abstraction over asynchronous waiting used by the delay simulator.
/// </summary>
public interface IJobDelayAwaiter
{
    /// <summary>
    /// Waits for the specified duration.
    /// </summary>
    Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken = default);
}
