using Hangfire;
using Hangfire.States;
using MiniApiServer.Application.Abstractions.Jobs;
using System.Linq.Expressions;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class HangfireBackgroundJobScheduler(
    IBackgroundJobClient backgroundJobClient,
    IBackgroundJobQueueResolver queueResolver) : IBackgroundJobScheduler
{
    public Task EnqueueProcessDivisionAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default)
    {
        EnqueueJob<ProcessDivisionJob>(job => job.ExecuteAsync(dataInId), category);
        return Task.CompletedTask;
    }

    public Task EnqueueProcessMultiplicationAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default)
    {
        EnqueueJob<ProcessMultiplicationJob>(job => job.ExecuteAsync(dataInId), category);
        return Task.CompletedTask;
    }

    public Task EnqueueProcessSubtractionAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default)
    {
        EnqueueJob<ProcessSubtractionJob>(job => job.ExecuteAsync(dataInId), category);
        return Task.CompletedTask;
    }

    public Task EnqueueProcessSumAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default)
    {
        EnqueueJob<ProcessSumJob>(job => job.ExecuteAsync(dataInId), category);
        return Task.CompletedTask;
    }

    private void EnqueueJob<TJob>(Expression<Func<TJob, Task>> methodCall, BackgroundJobCategory category)
    {
        var queue = queueResolver.ResolveQueue(category);
        backgroundJobClient.Create(methodCall, new EnqueuedState(queue));
    }
}
