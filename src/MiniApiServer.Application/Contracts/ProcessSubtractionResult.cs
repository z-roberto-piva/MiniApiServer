namespace MiniApiServer.Application.Contracts;

/// <summary>
/// Result returned by the subtraction use case.
/// </summary>
/// <param name="DataInId">Identifier of the processed input.</param>
/// <param name="Result">Computed subtraction.</param>
public sealed record ProcessSubtractionResult(Guid DataInId, int Result);
