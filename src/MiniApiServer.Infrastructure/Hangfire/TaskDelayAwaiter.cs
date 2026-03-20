namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class TaskDelayAwaiter : IJobDelayAwaiter
{
    public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken = default)
        => Task.Delay(delay, cancellationToken);
}
