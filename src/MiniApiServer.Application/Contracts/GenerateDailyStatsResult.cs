namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Result returned after the daily statistics snapshot has been generated.
/// </summary>
/// <param name="Date">Aggregated day.</param>
/// <param name="NumberOfOperations">Total number of operations included in the snapshot.</param>
/// <param name="TotalOfSums">Aggregate total for sums.</param>
/// <param name="TotalOfSubtractions">Aggregate total for subtractions.</param>
/// <param name="TotalOfMultiplications">Aggregate total for multiplications.</param>
/// <param name="TotalOfDivisions">Aggregate total for divisions.</param>
public sealed record GenerateDailyStatsResult(
    DateOnly Date,
    int NumberOfOperations,
    int TotalOfSums,
    int TotalOfSubtractions,
    int TotalOfMultiplications,
    int TotalOfDivisions);
