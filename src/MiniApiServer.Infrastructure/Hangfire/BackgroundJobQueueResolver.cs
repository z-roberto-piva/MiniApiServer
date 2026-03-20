using Microsoft.Extensions.Options;
using MiniApiServer.Application.Abstractions.Jobs;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class BackgroundJobQueueResolver(IOptions<HangfireQueueOptions> options) : IBackgroundJobQueueResolver
{
    public string ResolveQueue(BackgroundJobCategory category)
        => category switch
        {
            BackgroundJobCategory.HighPriority => options.Value.HighPriority,
            BackgroundJobCategory.StandardPriority => options.Value.StandardPriority,
            BackgroundJobCategory.LowPriority => options.Value.LowPriority,
            _ => options.Value.StandardPriority
        };

    public string[] GetWorkerQueuesInPriorityOrder()
        => options.Value.WorkerQueues.Length > 0
            ? options.Value.WorkerQueues
            : [options.Value.HighPriority, options.Value.StandardPriority, options.Value.LowPriority];
}
