namespace MiniApiServer.Application.Abstractions.Jobs;

public interface IBackgroundJobScheduler
{
    Task EnqueueProcessDivisionAsync(Guid dataInId, CancellationToken cancellationToken = default);

    Task EnqueueProcessMultiplicationAsync(Guid dataInId, CancellationToken cancellationToken = default);

    Task EnqueueProcessSumAsync(Guid dataInId, CancellationToken cancellationToken = default);

    Task EnqueueProcessSubtractionAsync(Guid dataInId, CancellationToken cancellationToken = default);
}
