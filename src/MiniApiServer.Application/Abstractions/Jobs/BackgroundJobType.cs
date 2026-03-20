namespace MiniApiServer.Application.Abstractions.Jobs;

public enum BackgroundJobType
{
    ProcessSum = 1,
    ProcessSubtraction = 2,
    ProcessMultiplication = 3,
    ProcessDivision = 4,
    GenerateDailyStats = 5
}
