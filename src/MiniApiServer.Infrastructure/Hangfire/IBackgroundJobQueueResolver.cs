using MiniApiServer.Application.Abstractions.Jobs;

namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Maps logical background job categories to Hangfire queue names.
/// </summary>
public interface IBackgroundJobQueueResolver
{
    /// <summary>
    /// Returns the queue name assigned to the specified category.
    /// </summary>
    string ResolveQueue(BackgroundJobCategory category);

    /// <summary>
    /// Returns the queue list that the worker should listen to, ordered by priority.
    /// </summary>
    string[] GetWorkerQueuesInPriorityOrder();
}
