namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Aggregated totals read from persistence for a given day.
/// </summary>
/// <param name="Date">Aggregated day.</param>
/// <param name="NumberOfOperations">Total number of processed operations.</param>
/// <param name="TotalOfSums">Aggregate total for sums.</param>
/// <param name="TotalOfSubtractions">Aggregate total for subtractions.</param>
/// <param name="TotalOfMultiplications">Aggregate total for multiplications.</param>
/// <param name="TotalOfDivisions">Aggregate total for divisions.</param>
public sealed record DailyOperationsSummary(
    DateOnly Date,
    int NumberOfOperations,
    int TotalOfSums,
    int TotalOfSubtractions,
    int TotalOfMultiplications,
    int TotalOfDivisions);
