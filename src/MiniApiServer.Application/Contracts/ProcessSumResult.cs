namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Result returned by the sum use case.
/// </summary>
/// <param name="DataInId">Identifier of the processed input.</param>
/// <param name="Result">Computed sum.</param>
public sealed record ProcessSumResult(Guid DataInId, int Result);
