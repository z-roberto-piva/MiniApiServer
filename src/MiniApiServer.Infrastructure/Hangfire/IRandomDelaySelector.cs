namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Selects a random index from the configured list of allowed delays.
/// </summary>
public interface IRandomDelaySelector
{
    /// <summary>
    /// Returns a random index between zero and the specified exclusive upper bound.
    /// </summary>
    int NextIndex(int upperBoundExclusive);
}
