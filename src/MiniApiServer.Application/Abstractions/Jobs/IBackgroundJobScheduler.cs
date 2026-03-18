namespace MiniApiServer.Application.Abstractions.Jobs;

public interface IBackgroundJobScheduler
{
    Task EnqueueProcessSumAsync(Guid dataInId, CancellationToken cancellationToken = default);

    Task EnqueueProcessSubtractionAsync(Guid dataInId, CancellationToken cancellationToken = default);
}
