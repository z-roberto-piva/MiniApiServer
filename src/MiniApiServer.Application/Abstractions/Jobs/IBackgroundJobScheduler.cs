namespace MiniApiServer.Application.Abstractions.Jobs;

public interface IBackgroundJobScheduler
{
    Task EnqueueProcessDivisionAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default);

    Task EnqueueProcessMultiplicationAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default);

    Task EnqueueProcessSumAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default);

    Task EnqueueProcessSubtractionAsync(
        Guid dataInId,
        BackgroundJobCategory category = BackgroundJobCategory.HighPriority,
        CancellationToken cancellationToken = default);
}
