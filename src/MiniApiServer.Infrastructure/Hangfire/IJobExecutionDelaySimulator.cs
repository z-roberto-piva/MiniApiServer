namespace MiniApiServer.Infrastructure.Hangfire;

public interface IJobExecutionDelaySimulator
{
    Task DelayAsync(string jobName, Guid dataInId, CancellationToken cancellationToken = default);
}
