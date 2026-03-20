namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Result returned by the multiplication use case.
/// </summary>
/// <param name="DataInId">Identifier of the processed input.</param>
/// <param name="Result">Computed multiplication.</param>
public sealed record ProcessMultiplicationResult(Guid DataInId, int Result);
