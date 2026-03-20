namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Command that requests generation of the statistics snapshot for a specific day.
/// </summary>
/// <param name="Date">Day to aggregate.</param>
public sealed record GenerateDailyStatsCommand(DateOnly Date);
