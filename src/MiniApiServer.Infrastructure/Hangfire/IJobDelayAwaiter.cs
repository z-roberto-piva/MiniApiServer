namespace MiniApiServer.Infrastructure.Hangfire;

public interface IJobDelayAwaiter
{
    Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken = default);
}
