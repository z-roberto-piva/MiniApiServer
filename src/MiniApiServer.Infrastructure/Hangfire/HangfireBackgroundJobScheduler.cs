using Hangfire;
using MiniApiServer.Application.Abstractions.Jobs;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class HangfireBackgroundJobScheduler(IBackgroundJobClient backgroundJobClient) : IBackgroundJobScheduler
{
    public Task EnqueueProcessDivisionAsync(Guid dataInId, CancellationToken cancellationToken = default)
    {
        backgroundJobClient.Enqueue<ProcessDivisionJob>(job => job.ExecuteAsync(dataInId));
        return Task.CompletedTask;
    }

    public Task EnqueueProcessMultiplicationAsync(Guid dataInId, CancellationToken cancellationToken = default)
    {
        backgroundJobClient.Enqueue<ProcessMultiplicationJob>(job => job.ExecuteAsync(dataInId));
        return Task.CompletedTask;
    }

    public Task EnqueueProcessSubtractionAsync(Guid dataInId, CancellationToken cancellationToken = default)
    {
        backgroundJobClient.Enqueue<ProcessSubtractionJob>(job => job.ExecuteAsync(dataInId));
        return Task.CompletedTask;
    }

    public Task EnqueueProcessSumAsync(Guid dataInId, CancellationToken cancellationToken = default)
    {
        backgroundJobClient.Enqueue<ProcessSumJob>(job => job.ExecuteAsync(dataInId));
        return Task.CompletedTask;
    }
}
