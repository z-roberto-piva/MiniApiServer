namespace MiniApiServer.Application.Abstractions.Jobs;

/// <summary>
/// Schedules background jobs without exposing the underlying Hangfire dependency.
/// </summary>
public interface IBackgroundJobScheduler
{
    /// <summary>
    /// Enqueues the division job for the specified input.
    /// </summary>
    Task EnqueueProcessDivisionAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues the multiplication job for the specified input.
    /// </summary>
    Task EnqueueProcessMultiplicationAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues the sum job for the specified input.
    /// </summary>
    Task EnqueueProcessSumAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues the subtraction job for the specified input.
    /// </summary>
    Task EnqueueProcessSubtractionAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default);
}
