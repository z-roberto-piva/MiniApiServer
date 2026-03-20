namespace MiniApiServer.Infrastructure.Hangfire;

public interface IRandomDelaySelector
{
    int NextIndex(int upperBoundExclusive);
}
