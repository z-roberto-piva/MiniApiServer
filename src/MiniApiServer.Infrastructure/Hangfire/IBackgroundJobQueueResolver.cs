using MiniApiServer.Application.Abstractions.Jobs;

namespace MiniApiServer.Infrastructure.Hangfire;

public interface IBackgroundJobQueueResolver
{
    string ResolveQueue(BackgroundJobCategory category);

    string[] GetWorkerQueuesInPriorityOrder();
}
