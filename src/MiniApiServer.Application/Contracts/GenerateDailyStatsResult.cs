namespace MiniApiServer.Application.Contracts;

public sealed record GenerateDailyStatsResult(DateOnly Date, int NumberOfOperations, int TotalOfSums, int TotalOfSubtractions);
