namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Result returned by the division use case.
/// </summary>
/// <param name="DataInId">Identifier of the processed input.</param>
/// <param name="Result">Computed division.</param>
public sealed record ProcessDivisionResult(Guid DataInId, int Result);
