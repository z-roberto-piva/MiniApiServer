namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class RandomDelaySelector : IRandomDelaySelector
{
    public int NextIndex(int upperBoundExclusive) => Random.Shared.Next(upperBoundExclusive);
}
