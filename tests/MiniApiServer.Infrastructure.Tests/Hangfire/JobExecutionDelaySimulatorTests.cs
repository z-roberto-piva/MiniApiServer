using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MiniApiServer.Infrastructure.Hangfire;

namespace MiniApiServer.Infrastructure.Tests.Hangfire;

public sealed class JobExecutionDelaySimulatorTests
{
    [Fact]
    public async Task DelayAsync_ShouldNotWaitWhenDisabled()
    {
        var selector = new FixedRandomDelaySelector(0);
        var awaiter = new RecordingJobDelayAwaiter();
        var options = Options.Create(new JobExecutionDelayOptions
        {
            Enabled = false,
            AllowedDurationsInSeconds = [5, 10, 15]
        });
        var simulator = new JobExecutionDelaySimulator(options, selector, awaiter, NullLogger<JobExecutionDelaySimulator>.Instance);

        await simulator.DelayAsync("sum", Guid.NewGuid());

        Assert.Empty(awaiter.Delays);
    }

    [Fact]
    public async Task DelayAsync_ShouldWaitUsingOneOfAllowedDurations()
    {
        var selector = new FixedRandomDelaySelector(2);
        var awaiter = new RecordingJobDelayAwaiter();
        var options = Options.Create(new JobExecutionDelayOptions
        {
            Enabled = true,
            AllowedDurationsInSeconds = [5, 10, 15, 999]
        });
        var simulator = new JobExecutionDelaySimulator(options, selector, awaiter, NullLogger<JobExecutionDelaySimulator>.Instance);

        await simulator.DelayAsync("division", Guid.NewGuid());

        Assert.Single(awaiter.Delays);
        Assert.Equal(TimeSpan.FromSeconds(15), awaiter.Delays.Single());
    }

    private sealed class FixedRandomDelaySelector(int index) : IRandomDelaySelector
    {
        public int NextIndex(int upperBoundExclusive) => Math.Min(index, upperBoundExclusive - 1);
    }

    private sealed class RecordingJobDelayAwaiter : IJobDelayAwaiter
    {
        public List<TimeSpan> Delays { get; } = [];

        public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken = default)
        {
            Delays.Add(delay);
            return Task.CompletedTask;
        }
    }
}
