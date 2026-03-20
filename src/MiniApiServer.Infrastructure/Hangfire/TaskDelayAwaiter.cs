namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Uses <see cref="Task.Delay(TimeSpan, CancellationToken)"/> to perform asynchronous waits.
/// </summary>
public sealed class TaskDelayAwaiter : IJobDelayAwaiter
{
    /// <inheritdoc />
    public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken = default)
        => Task.Delay(delay, cancellationToken);
}
