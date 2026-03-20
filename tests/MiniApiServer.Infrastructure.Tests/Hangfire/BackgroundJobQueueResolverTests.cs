using Microsoft.Extensions.Options;
using MiniApiServer.Application.Abstractions.Jobs;
using MiniApiServer.Infrastructure.Hangfire;

namespace MiniApiServer.Infrastructure.Tests.Hangfire;

public sealed class BackgroundJobQueueResolverTests
{
    [Fact]
    public void ResolveQueue_ShouldMapCategoriesToConfiguredQueues()
    {
        var options = Options.Create(new HangfireQueueOptions
        {
            HighPriority = "critical",
            StandardPriority = "default",
            LowPriority = "maintenance",
            WorkerQueues = ["critical", "default", "maintenance"]
        });
        var resolver = new BackgroundJobQueueResolver(options);

        Assert.Equal("critical", resolver.ResolveQueue(BackgroundJobCategory.HighPriority));
        Assert.Equal("default", resolver.ResolveQueue(BackgroundJobCategory.StandardPriority));
        Assert.Equal("maintenance", resolver.ResolveQueue(BackgroundJobCategory.LowPriority));
    }

    [Fact]
    public void GetWorkerQueuesInPriorityOrder_ShouldReturnConfiguredOrder()
    {
        var options = Options.Create(new HangfireQueueOptions
        {
            WorkerQueues = ["critical", "default", "maintenance"]
        });
        var resolver = new BackgroundJobQueueResolver(options);

        Assert.Equal(["critical", "default", "maintenance"], resolver.GetWorkerQueuesInPriorityOrder());
    }
}
