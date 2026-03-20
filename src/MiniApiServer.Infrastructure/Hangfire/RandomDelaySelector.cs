namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Default random selector used by the delay simulator.
/// </summary>
public sealed class RandomDelaySelector : IRandomDelaySelector
{
    /// <inheritdoc />
    public int NextIndex(int upperBoundExclusive) => Random.Shared.Next(upperBoundExclusive);
}
