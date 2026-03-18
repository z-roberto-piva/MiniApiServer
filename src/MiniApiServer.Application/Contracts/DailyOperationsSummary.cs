namespace MiniApiServer.Application.Contracts;

public sealed record DailyOperationsSummary(DateOnly Date, int NumberOfOperations, int TotalOfSums, int TotalOfSubtractions);
